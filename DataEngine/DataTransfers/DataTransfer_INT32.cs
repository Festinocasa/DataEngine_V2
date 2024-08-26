namespace DataEngine.DataTransfers
{
    internal sealed class DataTransfer_INT32 : DataTransfer<int>
    {
        internal override int ParseFromBinary(byte[] bytes)
        {
            return BitConverter.ToInt32(bytes, 0);
        }

        internal override int ParseFromString(string str)
        {
            if (int.TryParse(str, out int result))
                return result;
            else
                throw new ArgumentException($"Failed to parse '{str}' as Int32 (int).");
        }

        internal override byte[] ParseToBinary(int value)
        {
            return BitConverter.GetBytes(value);
        }
    }
}
