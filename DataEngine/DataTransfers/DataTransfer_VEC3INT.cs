using DataEngine.Protocal;
using DataEngine.Utility;

namespace DataEngine.DataTransfers
{
    internal sealed class DataTransfer_VEC3INT : DataTransfer<Vector3Int>
    {
        internal override Vector3Int ParseFromBinary(byte[] bytes)
        {
            int sizeOfFloat = sizeof(int);

            int point = 0;

            int x = BitConverter.ToInt32(bytes, point);
            point += sizeOfFloat;
            int y = BitConverter.ToInt32(bytes, point);
            point += sizeOfFloat;
            int z = BitConverter.ToInt32(bytes, point);
            point += sizeOfFloat;

            return new Vector3Int(x, y, z);

        }

        internal override Vector3Int ParseFromString(string str)
        {
            string[] dataStrs = str.Split('|');
            int x = int.Parse(dataStrs[0]);
            int y = int.Parse(dataStrs[1]);
            int z = int.Parse(dataStrs[2]);
            return new Vector3Int(x, y, z);
        }

        internal override byte[] ParseToBinary(Vector3Int value)
        {
            int sizeOfInt = sizeof(int);
            int sizeOfVec3 = 3 * sizeOfInt;

            int point = 0;

            byte[] result = new byte[sizeOfVec3];

            result.AddArrayRange(point, BitConverter.GetBytes(value.x));
            point += sizeOfInt;
            result.AddArrayRange(point, BitConverter.GetBytes(value.y));
            point += sizeOfInt;
            result.AddArrayRange(point, BitConverter.GetBytes(value.z));
            point += sizeOfInt;

            return result;
        }
    }

}
