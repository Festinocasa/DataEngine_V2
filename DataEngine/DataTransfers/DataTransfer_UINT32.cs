namespace DataEngine.DataTransfers
{
    internal sealed class DataTransfer_UINT32 : DataTransfer<uint>
    {
        internal override uint ParseFromBinary(byte[] bytes)
        {
            return BitConverter.ToUInt32(bytes, 0);
        }

        internal override uint ParseFromString(string str)
        {
            if (uint.TryParse(str, out uint result))
                return result;
            else
                throw new ArgumentException($"Failed to parse '{str}' as UInt32 (uint).");
        }

        internal override byte[] ParseToBinary(uint value)
        {
            return BitConverter.GetBytes(value);
        }
    }
}
