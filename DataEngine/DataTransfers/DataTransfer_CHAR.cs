namespace DataEngine.DataTransfers
{
    internal class DataTransfer_CHAR : DataTransfer<char>
    {
        internal override char ParseFromBinary(byte[] bytes)
        {
            return BitConverter.ToChar(bytes);
        }

        internal override char ParseFromString(string str)
        {
            if (str.Length != 1)
                throw new Exception("Char is not char");

            return str[0];

        }

        internal override byte[] ParseToBinary(char value)
        {
            return BitConverter.GetBytes(value);
        }
    }




}
