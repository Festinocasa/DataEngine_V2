using System.Text;

namespace DataEngine.FieldTypes
{
    public class FieldStringType : FieldType
    {
        internal FieldStringType(string fieldConfigName, int tableBodyByteSize) : base(fieldConfigName, tableBodyByteSize) { }

        public override Type TypeInfo => typeof(String);

        public override object ParseObjectFromByteArr(byte[] bytesData)
        {
            return Encoding.UTF8.GetString(bytesData);
        }

        public override byte[] ParseByteArrFromObject(object obj)
        {
            return Encoding.UTF8.GetBytes((string)obj);
        }

        public override object ParseObjectFromString(string stringData)
        {
            return stringData;
        }

        public byte[] StringToButeArr(string stringData) =>
          UTF8Encoding.UTF8.GetBytes(stringData);
    }




}
