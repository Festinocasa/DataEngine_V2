using DataEngine.MetaTypes;
using DataEngine.Utility;

namespace DataEngine
{
    public class MetaTypeManager : Singleton<MetaTypeManager>
    {
        private Dictionary<ushort, MetaType> metaTypeDic;

        protected override void OnInit()
        {
            metaTypeDic = new Dictionary<ushort, MetaType>();
            metaTypeDic.Add(META_TYPE_ID.ENUMID, new MetaTypeEnumId("ENUMID"));
            metaTypeDic.Add(META_TYPE_ID.NAVID, new MetaTypeNavId("NAVID"));
            metaTypeDic.Add(META_TYPE_ID.MAPID, new MetaTypeMapId("MAPID"));
            metaTypeDic.Add(META_TYPE_ID.VALUE, new MetaTypeValue("VALUE"));
            metaTypeDic.Add(META_TYPE_ID.ARR, new MetaTypeArr("ARR"));
        }

        public MetaType GetMataType(ushort metaTypeId)
        {
            return metaTypeDic[metaTypeId];
        }

        public MetaType GetMetaType(string metaTypeName)
        {
            return metaTypeDic.Values.First(m => m.ConfigName == metaTypeName);
        }

        public ushort GetMetaTypeId(string metaTypeName)
        {
            return metaTypeDic.First(pair => pair.Value.ConfigName == metaTypeName).Key;
        }

    }

}
