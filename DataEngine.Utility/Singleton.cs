namespace DataEngine.Utility
{
    public abstract class Singleton<T> : IDisposable where T : class, new()
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                    instance = new T();
                return instance;
            }
        }

        protected Singleton() =>
            OnInit();

        public virtual void Dispose() =>
            OnDispose();

        protected virtual void OnInit() { }
        protected virtual void OnDispose() { }
    }
}