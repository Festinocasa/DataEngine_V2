using DataEngine.Protocal;
using DataEngine.Utility;

namespace DataEngine.DataTransfers
{
    internal sealed class DataTransfer_VEC2 : DataTransfer<Vector2>
    {
        internal override Vector2 ParseFromBinary(byte[] bytes)
        {
            float x = BitConverter.ToSingle(bytes, 0);
            float y = BitConverter.ToSingle(bytes, 4);
            return new Vector2(x, y);
        }

        internal override Vector2 ParseFromString(string str)
        {
            string[] dataXY = str.Split('*');
            float x = float.Parse(dataXY[0]);
            float y = float.Parse(dataXY[1]);
            return new Vector2(x, y);
        }

        internal override byte[] ParseToBinary(Vector2 value)
        {
            int floatSize = sizeof(float);
            int vecSize = 2 * floatSize;
            byte[] result = new byte[vecSize];
            int point = 0;
            result.AddArrayRange(point, BitConverter.GetBytes(value.x));
            point += floatSize;
            result.AddArrayRange(point, BitConverter.GetBytes(value.y));
            return result;
        }
    }
}
