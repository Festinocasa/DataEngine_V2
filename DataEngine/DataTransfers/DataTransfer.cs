namespace DataEngine.DataTransfers
{
    internal abstract class DataTransfer<T> where T : struct
    {
        internal DataTransfer() { }

        internal abstract T ParseFromBinary(byte[] bytes);
        internal abstract T ParseFromString(string str);
        internal abstract byte[] ParseToBinary(T value);
    }




}
