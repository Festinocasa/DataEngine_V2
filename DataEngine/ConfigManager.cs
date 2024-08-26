using DataEngine.FieldTypes;
using DataEngine.MetaTypes;
using DataEngine.Utility;
using System.Reflection;
using System.Text;

namespace DataEngine
{
    public class ConfigManager : Singleton<ConfigManager>
    {
        private Version version;
        private Dictionary<Type, Dictionary<object, object>> configDataDic;
        private Dictionary<Type, object> configDataSetTempDic;

        protected override void OnInit()
        {
            configDataDic = new Dictionary<Type, Dictionary<object, object>>();
            configDataSetTempDic = new Dictionary<Type, object>();
        }

        public async Task LoadDataFileAsync(string filePath, string modelAssemblyName, IProgress<double> progress)
        {
            var data = await LoadData(filePath, progress);
            ParseData(modelAssemblyName, data);
        }

        private async Task<byte[]> LoadData(string filePath, IProgress<double> progress)
        {
            const int bufferSize = 64;
            byte[] buffer = new byte[bufferSize];

            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, true))
            {
                long totalBytesRead = 0;
                long totalBytes = fs.Length;
                var ms = new MemoryStream();

                var semaphoreSlim = new SemaphoreSlim(1, 1);

                while (totalBytesRead < totalBytes)
                {
                    int bytesRead = await fs.ReadAsync(buffer, 0, bufferSize);

                    if (bytesRead == 0) // 文件已读完
                        break;

                    // 将已读数据写入内存流
                    ms.Write(buffer, 0, bytesRead);
                    totalBytesRead += bytesRead;

                    await semaphoreSlim.WaitAsync();

                    try
                    {
                        // 报告进度
                        if (progress != null)
                        {
                            double percent = (double)totalBytesRead / totalBytes;
                            progress.Report(percent);
                        }
                    }
                    finally
                    {
                        semaphoreSlim.Release();
                    }

                }

                return ms.ToArray();
            }

        }

        private void ParseData(string modelAssemblyName, byte[] data)
        {
            var modelAssembly = Assembly.Load(modelAssemblyName);

            var point = 0;
            var sizeOfInt = sizeof(int);
            var sizeOfUint = sizeof(uint);
            var sizeOfUshort = sizeof(ushort);

            var major = BitConverter.ToInt32(data, point);
            point += sizeOfInt;
            var minor = BitConverter.ToInt32(data, point);
            point += sizeOfInt;
            var build = BitConverter.ToInt32(data, point);
            point += sizeOfInt;
            var revision = BitConverter.ToInt32(data, point);
            point += sizeOfInt;


            int fieldTableAddr = BitConverter.ToInt32(data, point);
            point += sizeOfInt;
            int modelTableAddr = BitConverter.ToInt32(data, point);
            point += sizeOfInt;
            int elemTableAddr = BitConverter.ToInt32(data, point);
            point += sizeOfInt;
            int elemTableValueAddr = BitConverter.ToInt32(data, point);
            point += sizeOfInt;

            int fieldTableElemCount = BitConverter.ToInt32(data, point);
            point += sizeOfInt;
            int fieldTableElemByteLen = BitConverter.ToInt32(data, point);
            point += sizeOfInt;
            Dictionary<uint, Tuple<ushort, ushort>> fieldTableBodyDic = new Dictionary<uint, Tuple<ushort, ushort>>();

            for (int i = 0; i < fieldTableElemCount; i++)
            {
                var id = BitConverter.ToUInt32(data, point);
                point += sizeOfUint;
                var fieldTypeId = BitConverter.ToUInt16(data, point);
                point += sizeOfUshort;
                var fieldMetaTypeId = BitConverter.ToUInt16(data, point);
                point += sizeOfUshort;

                fieldTableBodyDic.Add(id, new Tuple<ushort, ushort>(fieldTypeId, fieldMetaTypeId));
            }

            int modelTableElemCount = BitConverter.ToInt32(data, point);
            point += sizeOfInt;
            int modelTableElemByteLen = BitConverter.ToInt32(data, point);
            point += sizeOfInt;
            int modelTableValueAreaAddr = BitConverter.ToInt32(data, point);
            point += sizeOfInt;
            List<Tuple<int, int, int, int, int>> modelTableBodyDic = new List<Tuple<int, int, int, int, int>>();
            List<Tuple<string, uint[]>> modelTableValueAreaDic = new List<Tuple<string, uint[]>>();

            var valueAreaAddrGlobal = point + modelTableValueAreaAddr;
            for (int i = 0; i < modelTableElemCount; i++)
            {
                var modelNameStrAddr = BitConverter.ToInt32(data, point);
                point += sizeOfInt;
                var modelFieldListAddr = BitConverter.ToInt32(data, point);
                point += sizeOfInt;
                var modelElemsListAddr = BitConverter.ToInt32(data, point);
                point += sizeOfInt;
                var modelElemsCount = BitConverter.ToInt32(data, point);
                point += sizeOfInt;
                var modelElemByteLen = BitConverter.ToInt32(data, point);
                point += sizeOfInt;

                modelTableBodyDic.Add(new Tuple<int, int, int, int, int>(modelNameStrAddr, modelFieldListAddr, modelElemsListAddr, modelElemsCount, modelElemByteLen));

                var strLen = BitConverter.ToInt32(data, valueAreaAddrGlobal);
                valueAreaAddrGlobal += sizeOfInt;

                var subCharByteArr = new byte[strLen];
                Array.Copy(data, valueAreaAddrGlobal, subCharByteArr, 0, strLen);
                var strModelName = Encoding.UTF8.GetString(subCharByteArr);
                valueAreaAddrGlobal += strLen;


                var fieldListLen = BitConverter.ToInt32(data, valueAreaAddrGlobal);
                valueAreaAddrGlobal += sizeOfInt;
                var idArr = new uint[fieldListLen];
                for (int j = 0; j < fieldListLen; j++)
                {
                    idArr[j] = BitConverter.ToUInt32(data, valueAreaAddrGlobal);
                    valueAreaAddrGlobal += sizeOfUint;
                }

                modelTableValueAreaDic.Add(new Tuple<string, uint[]>(strModelName, idArr));
            }

            point = elemTableAddr;
            valueAreaAddrGlobal = elemTableValueAddr;
            for (int i = 0; i < modelTableElemCount; i++)
            {
                var modelBody = modelTableBodyDic[i];
                var modelValue = modelTableValueAreaDic[i];

                string typeName = "DataEngine.Models." + modelValue.Item1;
                var modelType = modelAssembly.GetType(typeName);

                var fieldsInfo = (from id in modelValue.Item2 select fieldTableBodyDic[id]).ToArray();
                var typeElemDic = new Dictionary<object, object>();

                for (int row = 0; row < modelBody.Item4; row++)
                {
                    var paramObjs = new object[modelValue.Item2.Length];
                    for (int col = 0; col < fieldsInfo.Length; col++)
                    {
                        var fieldInfo = fieldsInfo[col];
                        var fieldType = FieldTypeManager.Instance.GetFieldType(fieldInfo.Item1);
                        var metaFieldType = MetaTypeManager.Instance.GetMataType(fieldInfo.Item2);

                        if (metaFieldType is MetaTypeArr)
                        {
                            point += sizeOfInt;
                            if (fieldType is FieldStringType)
                            {
                                var strArrLen = BitConverter.ToInt32(data, valueAreaAddrGlobal);
                                valueAreaAddrGlobal += sizeOfInt;
                                var strArr = new string[strArrLen];
                                for (int k = 0; k < strArrLen; k++)
                                {
                                    var charByteArrLen = BitConverter.ToInt32(data, valueAreaAddrGlobal);
                                    valueAreaAddrGlobal += sizeOfInt;
                                    var charArr = new byte[charByteArrLen];
                                    Array.Copy(data, valueAreaAddrGlobal, charArr, 0, charByteArrLen);
                                    strArr[k] = Encoding.UTF8.GetString(charArr);
                                    valueAreaAddrGlobal += charByteArrLen;
                                }
                                paramObjs[col] = strArr;
                            }
                            else
                            {
                                var elemArrLen = BitConverter.ToInt32(data, valueAreaAddrGlobal);
                                valueAreaAddrGlobal += sizeOfInt;
                                var elemArr = new object[elemArrLen];
                                for (int k = 0; k < elemArrLen; k++)
                                {
                                    var valueBytes = new byte[fieldType.TableBodyByteSize];
                                    Array.Copy(data, valueAreaAddrGlobal, valueBytes, 0, fieldType.TableBodyByteSize);
                                    elemArr[k] = fieldType.ParseObjectFromByteArr(valueBytes);
                                    valueAreaAddrGlobal += fieldType.TableBodyByteSize;
                                }
                                paramObjs[col] = elemArr;
                            }
                        }
                        else
                        {
                            if (fieldType is FieldStringType)
                            {
                                var charByteArrLen = BitConverter.ToInt32(data, valueAreaAddrGlobal);
                                valueAreaAddrGlobal += sizeOfInt;
                                var charArr = new byte[charByteArrLen];
                                Array.Copy(data, valueAreaAddrGlobal, charArr, 0, charByteArrLen);
                                var str = Encoding.UTF8.GetString(charArr);
                                paramObjs[col] = str;
                                point += sizeOfInt;
                                valueAreaAddrGlobal += charByteArrLen;
                            }
                            else
                            {
                                var valueBytes = new byte[fieldType.TableBodyByteSize];
                                Array.Copy(data, point, valueBytes, 0, fieldType.TableBodyByteSize);
                                var val = fieldType.ParseObjectFromByteArr(valueBytes);
                                paramObjs[col] = val;
                                point += fieldType.TableBodyByteSize;
                            }

                        }
                    }
                    var configObj = Activator.CreateInstance(modelType, new object[] { paramObjs });
                    typeElemDic.Add(paramObjs[0], configObj);
                }

                configDataDic.Add(modelType, typeElemDic);
            }
        }

        public bool GetConfig<T>(object configId, out T value)
        {
            value = default;
            var type = typeof(T);
            if (configDataDic.TryGetValue(type, out var valueDic))
            {
                if (valueDic.TryGetValue(configId, out var valueObj))
                {
                    value = (T)valueObj;
                    return true;
                }
            }
            return false;
        }

        public bool GetConfigSet<T>(out T[] value)
        {
            value = default;
            var type = typeof(T);

            if (configDataSetTempDic.TryGetValue(type, out var arrObj))
            {
                value = (T[])arrObj;
                return true;
            }

            if (configDataDic.TryGetValue(type, out var objDic))
            {
                value = objDic.Values.Select(obj => (T)obj).ToArray();
                if (value != null)
                {
                    configDataSetTempDic.Add(type, value);
                    return true;
                }
            }

            return false;

        }

    }

}
