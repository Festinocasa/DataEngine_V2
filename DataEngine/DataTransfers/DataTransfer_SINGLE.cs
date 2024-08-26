namespace DataEngine.DataTransfers
{
    internal sealed class DataTransfer_SINGLE : DataTransfer<float>
    {
        internal override float ParseFromBinary(byte[] bytes)
        {
            return BitConverter.ToSingle(bytes, 0);
        }

        internal override float ParseFromString(string str)
        {
            if (float.TryParse(str, out float result))
                return result;
            else
                throw new ArgumentException($"Failed to parse '{str}' as Single (float).");
        }

        internal override byte[] ParseToBinary(float value)
        {
            return BitConverter.GetBytes(value);
        }
    }
}
