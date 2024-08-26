using DataEngine.Protocal;
using DataEngine.Utility;

namespace DataEngine.DataTransfers
{
    internal sealed class DataTransfer_VEC3 : DataTransfer<Vector3>
    {
        internal override Vector3 ParseFromBinary(byte[] bytes)
        {
            int sizeOfFloat = sizeof(float);

            int point = 0;

            float x = BitConverter.ToSingle(bytes, point);
            point += sizeOfFloat;
            float y = BitConverter.ToSingle(bytes, point);
            point += sizeOfFloat;
            float z = BitConverter.ToSingle(bytes, point);
            point += sizeOfFloat;

            return new Vector3(x, y, z);

        }

        internal override Vector3 ParseFromString(string str)
        {
            string[] dataStrs = str.Split('|');
            float x = float.Parse(dataStrs[0]);
            float y = float.Parse(dataStrs[1]);
            float z = float.Parse(dataStrs[2]);
            return new Vector3(x, y, z);
        }

        internal override byte[] ParseToBinary(Vector3 value)
        {
            int sizeOfFloat = sizeof(float);
            int sizeOfVec3 = sizeOfFloat;

            int point = 0;

            byte[] result = new byte[sizeOfVec3];

            result.AddArrayRange(point, BitConverter.GetBytes(value.x));
            point += sizeOfFloat;
            result.AddArrayRange(point, BitConverter.GetBytes(value.y));
            point += sizeOfFloat;
            result.AddArrayRange(point, BitConverter.GetBytes(value.z));
            point += sizeOfFloat;

            return result;
        }
    }

}
