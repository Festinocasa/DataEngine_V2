using DataEngine.FieldTypes;
using DataEngine.MetaTypes;
using System.Text;

namespace DataEngine.Generator
{
    internal class DataGeneratorContext
    {
        private Dictionary<string, uint> elementIdDic = new Dictionary<string, uint>();
        private Dictionary<string, uint> fieldIdDic = new Dictionary<string, uint>();

        public Dictionary<string, uint> ElementIdDic { get => elementIdDic; }
        public Dictionary<string, uint> FieldIdDic { get => fieldIdDic; }
    }

    internal class DataFileWriter
    {
        private uint fieldIdIndex;
        private uint elementIdIndex;
        private int lastSheetElementAddress;
        private int fieldInfoCount;
        private int sheetInfoCount;
        private DataGeneratorContext context;
        private List<byte> dataHeadBytes;
        private List<byte> fieldInfoTableHeadBytes;
        private List<byte> fieldInfoTableBodyBytes;
        private List<byte> sheetInfoTableHeadBytes;
        private List<byte> sheetInfoTableBodyBytes;
        private List<byte> sheetInfoTableValueArea;
        private List<byte> elementInfoTableBodyBytes;
        private List<byte> elementInfoTableValueArea;

        public DataFileWriter(DataGeneratorContext context)
        {
            this.context = context;

            lastSheetElementAddress = 0;

            dataHeadBytes = new List<byte>();
            fieldInfoTableHeadBytes = new List<byte>();
            fieldInfoTableBodyBytes = new List<byte>();
            sheetInfoTableHeadBytes = new List<byte>();
            sheetInfoTableBodyBytes = new List<byte>();
            sheetInfoTableValueArea = new List<byte>();
            elementInfoTableBodyBytes = new List<byte>();
            elementInfoTableValueArea = new List<byte>();
        }

        public void WriteFieldInfo(FieldInfo fieldInfo)
        {
            //add field elemId
            if (!context.FieldIdDic.TryGetValue(fieldInfo.FieldName, out var fieldId))
            {
                fieldId = fieldIdIndex++;
                context.FieldIdDic.Add(fieldInfo.FieldName, fieldId);
            }
            fieldInfoTableBodyBytes.AddRange(BitConverter.GetBytes(fieldId));

            //add field type elemId
            fieldInfoTableBodyBytes.AddRange(BitConverter.GetBytes(FieldTypeManager.Instance.GetFieldTypeId(fieldInfo.FieldTypeName)));

            //add field metatype elemId
            fieldInfoTableBodyBytes.AddRange(BitConverter.GetBytes(MetaTypeManager.Instance.GetMetaTypeId(fieldInfo.FieldMetaTypeName)));

            fieldInfoCount++;
        }

        public void WriteSheetInfo(SheetInfo sheetInfo)
        {

            //add sheet model name
            var sheetModelNameStringAddr = sheetInfoTableValueArea.Count;
            sheetInfoTableBodyBytes.AddRange(BitConverter.GetBytes(sheetModelNameStringAddr));
            sheetInfoTableValueArea.AddRange(FormatString(sheetInfo.SheetModelName));

            //add sheet field elemId list
            var sheetFieldListAddr = sheetInfoTableValueArea.Count;
            sheetInfoTableBodyBytes.AddRange(BitConverter.GetBytes(sheetFieldListAddr));
            var filedList = new List<uint>();
            foreach (var fieldTypeName in sheetInfo.FieldStrings)
                filedList.Add(context.FieldIdDic[fieldTypeName]);
            sheetInfoTableValueArea.AddRange(FormaFieldtList(filedList));

            //add sheet element address
            sheetInfoTableBodyBytes.AddRange(BitConverter.GetBytes(lastSheetElementAddress));
            lastSheetElementAddress = elementInfoTableValueArea.Count;

            //add sheet element count
            sheetInfoTableBodyBytes.AddRange(BitConverter.GetBytes(sheetInfo.ElemCount));

            //add sheet element byte length
            sheetInfoTableBodyBytes.AddRange(BitConverter.GetBytes(sheetInfo.ElemByteLen));

            sheetInfoCount++;
        }

        public void WriteElement(ElementInfo elementInfo)
        {
            for (int i = 0; i < elementInfo.ElementValues.Count; i++)
            {
                var valueStr = elementInfo.ElementValues[i];
                var typeStr = elementInfo.ElementFieldsType[i];
                var metaTypeStr = elementInfo.ElementMetaFieldsType[i];

                var fieldType = FieldTypeManager.Instance.GetFieldType(typeStr);
                var metaType = MetaTypeManager.Instance.GetMetaType(metaTypeStr);

                switch (metaType)
                {
                    case MetaTypeEnumId:

                        //body         | enumId |

                        if (!context.ElementIdDic.TryGetValue(valueStr, out var elemId))
                        {
                            elemId = elementIdIndex++;
                            context.ElementIdDic.Add(valueStr, elemId);
                        }
                        elementInfoTableBodyBytes.AddRange(BitConverter.GetBytes(elemId));
                        break;
                    case MetaTypeNavId:

                        //body         | navId |

                        if (!context.ElementIdDic.TryGetValue(valueStr, out elemId))
                        {
                            elemId = elementIdIndex++;
                            context.ElementIdDic.Add(valueStr, elemId);
                        }
                        elementInfoTableBodyBytes.AddRange(BitConverter.GetBytes(elemId));
                        break;
                    case MetaTypeMapId:

                        //body         | mapId |

                        if (fieldType is FieldStringType)
                        {
                            elementInfoTableBodyBytes.AddRange(BitConverter.GetBytes(elementInfoTableValueArea.Count));
                            elementInfoTableValueArea.AddRange(FormatString(valueStr));
                        }
                        else
                            elementInfoTableBodyBytes.AddRange(fieldType.ParseByteArrFromObject(fieldType.ParseObjectFromString(valueStr)));
                        break;
                    case MetaTypeValue:

                        if (fieldType is FieldStringType)
                        {

                            //body         | strValueAddr |
                            //valuearea    | strValue     |

                            elementInfoTableBodyBytes.AddRange(BitConverter.GetBytes(elementInfoTableValueArea.Count));
                            elementInfoTableValueArea.AddRange(FormatString(valueStr));
                        }
                        else
                        {
                            //body         | value |

                            elementInfoTableBodyBytes.AddRange(fieldType.ParseByteArrFromObject(fieldType.ParseObjectFromString(valueStr)));
                        }
                        break;
                    case MetaTypeArr typeArr:
                        var elementStrs = valueStr.Split('|');
                        if (elementStrs.Length == 0)
                            break;
                        if (fieldType is FieldStringType)
                        {

                            //body         | address |
                            //valuearea    | length  | formatStr1 | formatStr2 | formatStr3 | .... |

                            var stringBytesList = new List<byte>();
                            elementInfoTableBodyBytes.AddRange(BitConverter.GetBytes(elementInfoTableValueArea.Count));
                            elementInfoTableValueArea.AddRange(BitConverter.GetBytes(elementStrs.Length));
                            foreach (var elem in elementStrs)
                                elementInfoTableValueArea.AddRange(FormatString(elem));
                        }
                        else
                        {
                            //body         | address |
                            //valuearea    | length  | value1 | value2 | value3 |  ...  |

                            elementInfoTableBodyBytes.AddRange(BitConverter.GetBytes(elementInfoTableValueArea.Count));
                            elementInfoTableValueArea.AddRange(BitConverter.GetBytes(elementStrs.Length));
                            foreach (var elemStr in elementStrs)
                            {
                                var obj = fieldType.ParseObjectFromString(elemStr);
                                var byteArr = fieldType.ParseByteArrFromObject(obj);
                                elementInfoTableValueArea.AddRange(byteArr);
                            }
                        }
                        break;

                    default:
                        break;
                }

            }

        }

        public void FlushToFile(Version version, string exportFilePath)
        {
            var sizeOfInt = sizeof(int);
            var stream = new List<byte>();

            //add field info len
            fieldInfoTableHeadBytes.AddRange(BitConverter.GetBytes(fieldInfoCount));
            //add field info element byte size
            fieldInfoTableHeadBytes.AddRange(BitConverter.GetBytes(3 * sizeOfInt));
            //add sheet info length
            sheetInfoTableHeadBytes.AddRange(BitConverter.GetBytes(sheetInfoCount));
            //add sheet element byte size
            sheetInfoTableHeadBytes.AddRange(BitConverter.GetBytes(5 * sizeOfInt));
            //add sheet value area address
            sheetInfoTableHeadBytes.AddRange(BitConverter.GetBytes(sheetInfoTableBodyBytes.Count));

            //add data version
            dataHeadBytes.AddRange(BitConverter.GetBytes(version.Major));
            dataHeadBytes.AddRange(BitConverter.GetBytes(version.Minor));
            dataHeadBytes.AddRange(BitConverter.GetBytes(version.Build));
            dataHeadBytes.AddRange(BitConverter.GetBytes(version.Revision));

            int dataHeadByteLen = 8 * sizeOfInt;
            //add data field info table address
            dataHeadBytes.AddRange(BitConverter.GetBytes(dataHeadByteLen));
            //add data class info table address
            dataHeadBytes.AddRange(BitConverter.GetBytes(dataHeadByteLen + fieldInfoTableHeadBytes.Count + fieldInfoTableBodyBytes.Count));
            //add data element info table address
            dataHeadBytes.AddRange(BitConverter.GetBytes(dataHeadByteLen + fieldInfoTableHeadBytes.Count + fieldInfoTableBodyBytes.Count
                + sheetInfoTableHeadBytes.Count + sheetInfoTableBodyBytes.Count + sheetInfoTableValueArea.Count));
            //add data element info value address
            dataHeadBytes.AddRange(BitConverter.GetBytes(dataHeadByteLen + fieldInfoTableHeadBytes.Count + fieldInfoTableBodyBytes.Count
                + sheetInfoTableHeadBytes.Count + sheetInfoTableBodyBytes.Count + sheetInfoTableValueArea.Count + elementInfoTableBodyBytes.Count));

            stream.AddRange(dataHeadBytes);
            stream.AddRange(fieldInfoTableHeadBytes);
            stream.AddRange(fieldInfoTableBodyBytes);
            stream.AddRange(sheetInfoTableHeadBytes);
            stream.AddRange(sheetInfoTableBodyBytes);
            stream.AddRange(sheetInfoTableValueArea);
            stream.AddRange(elementInfoTableBodyBytes);
            stream.AddRange(elementInfoTableValueArea);

            File.WriteAllBytes(exportFilePath, stream.ToArray());
        }


        private byte[] FormatString(string stringData)
        {
            // | charLen | char1 | char2 | char3 | ... |

            List<byte> results = new List<byte>();
            results.AddRange(BitConverter.GetBytes(stringData.Length));
            results.AddRange(Encoding.UTF8.GetBytes(stringData));
            return results.ToArray();
        }

        private byte[] FormaFieldtList(List<uint> listData)
        {
            var results = new List<byte>();
            results.AddRange(BitConverter.GetBytes(listData.Count));
            foreach (var elem in listData)
                results.AddRange(BitConverter.GetBytes(elem));
            return results.ToArray();
        }

    }

}
