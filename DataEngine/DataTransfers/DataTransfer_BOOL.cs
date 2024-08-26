namespace DataEngine.DataTransfers
{
    internal class DataTransfer_BOOL : DataTransfer<bool>
    {
        internal override bool ParseFromBinary(byte[] bytes)
        {
            return BitConverter.ToBoolean(bytes);
        }

        internal override bool ParseFromString(string str)
        {
            return bool.Parse(str);
        }

        internal override byte[] ParseToBinary(bool value)
        {
            return BitConverter.GetBytes(value);
        }
    }




}
