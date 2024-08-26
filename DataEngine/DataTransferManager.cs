using DataEngine.DataTransfers;
using DataEngine.Utility;

namespace DataEngine
{
    internal class DataTransferManager : Singleton<DataTransferManager>
    {
        private Dictionary<Type, object> dataTransferDic;

        protected override void OnInit()
        {
            dataTransferDic = new Dictionary<Type, object>();

            //Add datatrasfer
            AddDataTransfer(new DataTransfer_INT32());
            AddDataTransfer(new DataTransfer_UINT32());
            AddDataTransfer(new DataTransfer_UINT16());
            AddDataTransfer(new DataTransfer_SINGLE());
            AddDataTransfer(new DataTransfer_VEC2());
            AddDataTransfer(new DataTransfer_VEC2INT());
            AddDataTransfer(new DataTransfer_VEC3());
            AddDataTransfer(new DataTransfer_VEC3INT());
            AddDataTransfer(new DataTransfer_CHAR());
            AddDataTransfer(new DataTransfer_BOOL());
        }

        private void AddDataTransfer<T>(DataTransfer<T> dataTransfer) where T : struct
        {
            dataTransferDic.Add(typeof(T), dataTransfer);
        }

        internal T ParseValueFromBinary<T>(byte[] bytes) where T : struct
        {
            DataTransfer<T> dataTransfer = dataTransferDic[typeof(T)] as DataTransfer<T>;
            return dataTransfer.ParseFromBinary(bytes);
        }

        internal T ParseValueFromString<T>(string str) where T : struct
        {
            DataTransfer<T> dataTransfer = dataTransferDic[typeof(T)] as DataTransfer<T>;
            return dataTransfer.ParseFromString(str);
        }

        internal byte[] ParseValueToBinary<T>(T value) where T : struct
        {
            DataTransfer<T> dataTransfer = dataTransferDic[typeof(T)] as DataTransfer<T>;
            return dataTransfer.ParseToBinary(value);
        }

        internal object GetDataTransfer(Type type)
        {
            return dataTransferDic[type];
        }
    }
}
