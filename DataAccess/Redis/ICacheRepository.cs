namespace DataAccess.Redis
{
    public interface ICacheRepository : IDisposable
    {
        Task<string> GetValueAsync(string key);
        Task<bool> SetValueAsync(string key, string value);
        Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> action) where T : class;
        T GetOrAdd<T>(string key, Func<T> action) where T : class;
        Task Clear(string key);
        void ClearAll();
        public bool DeleteByKey(string key);
        public TimeSpan? GetKeyTime(string key);
        public bool HSet(string key, string field, string value);
        public Dictionary<string, string> HGetAll(string key);
    }
}

