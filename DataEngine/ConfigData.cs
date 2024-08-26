namespace DataEngine.Models
{
    public abstract class ConfigData
    {
        public ConfigData(object[] objs) { }
        internal protected T[] ReadArray<T>(object obj)
        {
            var objArr = (object[])obj;
            return objArr.Select(o => (T)o).ToArray();
        }

        internal protected T ReadValue<T>(object obj)
        {
            return (T)obj;
        }
    }
}