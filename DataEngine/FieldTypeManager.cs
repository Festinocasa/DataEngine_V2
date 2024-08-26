using DataEngine.FieldTypes;
using DataEngine.Protocal;
using DataEngine.Utility;

namespace DataEngine
{
    public class FieldTypeManager : Singleton<FieldTypeManager>
    {
        private Dictionary<ushort, FieldType> fieldTypeDic;
        protected override void OnInit()
        {
            fieldTypeDic = new Dictionary<ushort, FieldType>();
            fieldTypeDic.Add(FIELD_TYPE_ID.UINT, new FieldValueType<uint>("UINT", sizeof(uint)));
            fieldTypeDic.Add(FIELD_TYPE_ID.INT, new FieldValueType<int>("INT", sizeof(int)));
            fieldTypeDic.Add(FIELD_TYPE_ID.FLOAT, new FieldValueType<float>("FLOAT", sizeof(float)));
            fieldTypeDic.Add(FIELD_TYPE_ID.CHAR, new FieldValueType<char>("CHAR", sizeof(char)));
            fieldTypeDic.Add(FIELD_TYPE_ID.BOOL, new FieldValueType<bool>("BOOL", sizeof(bool)));
            fieldTypeDic.Add(FIELD_TYPE_ID.VEC2, new FieldValueType<Vector2>("VEC2", 2 * sizeof(float)));
            fieldTypeDic.Add(FIELD_TYPE_ID.VEC2INT, new FieldValueType<Vector2Int>("VEC2INT", 2 * sizeof(int)));
            fieldTypeDic.Add(FIELD_TYPE_ID.VEC3, new FieldValueType<Vector3>("VEC3", 3 * sizeof(float)));
            fieldTypeDic.Add(FIELD_TYPE_ID.VEC3INT, new FieldValueType<Vector3Int>("VEC3INT", 3 * sizeof(int)));
            fieldTypeDic.Add(FIELD_TYPE_ID.STRING, new FieldStringType("STRING", sizeof(int)));
        }

        public FieldType GetFieldType(ushort fieldTypeID)
        {
            return fieldTypeDic[fieldTypeID];
        }

        public FieldType GetFieldType(string fieldConfigName)
        {
            return fieldTypeDic.Values.First(f => f.ConfigName == fieldConfigName.Replace(" ", ""));
        }

        public ushort GetFieldTypeId(string fieldConfigName)
        {
            return fieldTypeDic.First(pair => pair.Value.ConfigName == fieldConfigName.Replace(" ", "")).Key;
        }

    }

}
