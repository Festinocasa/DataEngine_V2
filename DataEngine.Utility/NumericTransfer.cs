namespace DataEngine.Utility
{
    public static class NumericTransfer
    {
        public static string ConvertUInt32ToBinaryString(uint n)
        {
            //0b_00000000_00000000_00000000_00000000
            return "0b_" + Convert.ToString(n, 2).PadLeft(32, '0').Insert(24, "_").Insert(16, "_").Insert(8, "_");
        }

        public static string ConvertUInt32ToHexString(uint n)
        {
            //0x_00_00_00_00
            return "0x_" + Convert.ToString(n, 16).PadLeft(8, '0').Insert(6, "_").Insert(4, "_").Insert(2, "_");
        }

        public static string ConvertUInt16ToHexString(ushort n)
        {
            //0x_00_00
            return "0x_" + Convert.ToString(n, 16).PadLeft(4, '0').Insert(2, "_");
        }

        public static string ConvertUInt16ToBinaryString(ushort n)
        {
            //0x_0000_0000__0000_0000
            return "0b_" + Convert.ToString(n, 2).PadLeft(16, '0').Insert(12, "_").Insert(8, "_").Insert(4, "_");
        }

    }
}
