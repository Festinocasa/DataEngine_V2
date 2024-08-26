using DataEngine.DataTransfers;

namespace DataEngine.FieldTypes
{
    public class FieldValueType<T> : FieldType where T : struct
    {
        private protected DataTransfer<T> dataTransfer;

        internal FieldValueType(string fieldConfigName, int tableBodyByteSize) : base(fieldConfigName, tableBodyByteSize)
        {
            dataTransfer = DataTransferManager.Instance.GetDataTransfer(TypeInfo) as DataTransfer<T>;
        }

        public override Type TypeInfo => typeof(T);


        public override object ParseObjectFromByteArr(byte[] bytesData)
        {
            return dataTransfer.ParseFromBinary(bytesData);
        }

        public override object ParseObjectFromString(string stringData)
        {
            return dataTransfer.ParseFromString(stringData);
        }

        public override byte[] ParseByteArrFromObject(object obj)
        {
            return dataTransfer.ParseToBinary((T)obj);
        }
    }




}
