using StackExchange.Redis;

namespace DataAccess.Redis
{
    public interface IRedisRepository : IDisposable
    {
        public string Get(string key);
        public bool DeleteByKey(string key);
        public Task<bool> DeleteByKeyAsync(string key);
        public bool DeleteByKeyLike(string key);
        public bool DeleteByPrefix(string prefix);
        public bool Clear();
        public Task<bool> SetAsync(string key, string value, TimeSpan? expireTime = null);
        public bool Set(string key, string value, TimeSpan? expireTime = null);
        public bool Exists(string key);
        public bool GetIfExists(string key, out string obj);
        public bool GetIfExistsObj(string key, out object obj);
        public bool ExistsLike(string key);
        public bool ExistsPrefixAndLike(string prefix, List<string> subTextList);
        public List<RedisKey> GetAllKeys();
        public List<RedisKey> GetAllKeysByLike(string key);
        public Dictionary<string, TimeSpan?> GetAllKeyTime();
        public TimeSpan? GetKeyTime(string key);
    }
}
