namespace DataEngine.MetaTypes
{

    public class MetaType
    {
        protected string configName;
        public string ConfigName { get => configName; }

        internal MetaType(string configName)
        {
            this.configName = configName;
        }

    }

    public class MetaTypeEnumId : MetaType
    {
        public MetaTypeEnumId(string configName) : base(configName)
        {
        }
    }

    public class MetaTypeNavId : MetaType
    {
        public MetaTypeNavId(string configName) : base(configName)
        {
        }
    }

    public class MetaTypeMapId : MetaType
    {
        public MetaTypeMapId(string configName) : base(configName)
        {
        }
    }

    public class MetaTypeValue : MetaType
    {
        public MetaTypeValue(string configName) : base(configName)
        {
        }
    }

    public class MetaTypeArr : MetaType
    {
        public MetaTypeArr(string configName) : base(configName)
        {
        }
    }

}
