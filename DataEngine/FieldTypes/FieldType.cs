namespace DataEngine.FieldTypes
{
    public abstract class FieldType
    {
        protected string configName;
        public int TableBodyByteSize { get; private set; }
        public abstract Type TypeInfo { get; }
        public string ConfigName { get => configName; }

        public abstract object ParseObjectFromString(string stringData);
        public abstract byte[] ParseByteArrFromObject(object obj);
        public abstract object ParseObjectFromByteArr(byte[] bytesData);

        internal FieldType(string fieldTypeConfigName, int tableBodyByteSize)
        {
            configName = fieldTypeConfigName;
            TableBodyByteSize = tableBodyByteSize;
        }
    }

}
