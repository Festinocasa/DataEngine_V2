using DataEngine.Protocal;
using DataEngine.Utility;

namespace DataEngine.DataTransfers
{
    internal sealed class DataTransfer_VEC2INT : DataTransfer<Vector2Int>
    {
        internal override Vector2Int ParseFromBinary(byte[] bytes)
        {
            int x = BitConverter.ToInt32(bytes, 0);
            int y = BitConverter.ToInt32(bytes, 4);
            return new Vector2Int(x, y);
        }

        internal override Vector2Int ParseFromString(string str)
        {
            string[] dataXY = str.Split('*');
            int x = int.Parse(dataXY[0]);
            int y = int.Parse(dataXY[1]);
            return new Vector2Int(x, y);
        }

        internal override byte[] ParseToBinary(Vector2Int value)
        {
            int intSize = sizeof(int);
            int vec2IntSize = 2 * intSize;
            byte[] result = new byte[vec2IntSize];
            int point = 0;
            result.AddArrayRange(point, BitConverter.GetBytes(value.x));
            point += intSize;
            result.AddArrayRange(point, BitConverter.GetBytes(value.y));
            return result;
        }
    }
}
