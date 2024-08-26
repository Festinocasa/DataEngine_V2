namespace DataEngine.FieldTypes
{
    public class FIELD_TYPE_ID
    {
        public const ushort UINT = 0b_0000_0000;
        public const ushort INT = 0b_0000_0001;
        public const ushort FLOAT = 0b_0000_0010;
        public const ushort CHAR = 0b_0000_0011;
        public const ushort BOOL = 0b_0000_0100;
        public const ushort VEC2 = 0b_0000_0101;
        public const ushort VEC2INT = 0b_0000_0110;
        public const ushort VEC3 = 0b_0000_0111;
        public const ushort VEC3INT = 0b_0000_1000;
        public const ushort STRING = 0b_0000_1001;
    }
}
