namespace DataEngine.DataTransfers
{
    internal sealed class DataTransfer_UINT16 : DataTransfer<ushort>
    {
        internal override ushort ParseFromBinary(byte[] bytes)
        {
            return BitConverter.ToUInt16(bytes, 0);
        }

        internal override ushort ParseFromString(string str)
        {
            if (ushort.TryParse(str, out ushort result))
                return result;
            else
                throw new ArgumentException($"Failed to parse '{str}' as UInt16 (ushort).");
        }

        internal override byte[] ParseToBinary(ushort value)
        {
            return BitConverter.GetBytes(value);
        }
    }
}
