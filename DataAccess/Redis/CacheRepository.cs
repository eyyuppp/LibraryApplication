using StackExchange.Redis;
using System.Text.Json;

namespace DataAccess.Redis
{
    public class CacheRepository : ICacheRepository
    {
        public static readonly string Domain = Environment.GetEnvironmentVariable("REDIS_URL");
        private readonly IConnectionMultiplexer _redisConnection = ConnectionMultiplexer.Connect(Domain);
        private readonly IDatabase _cache;
        private TimeSpan ExpireTime => TimeSpan.FromMinutes(1);
        public CacheRepository()
        {
            _cache = _redisConnection.GetDatabase();
        }
        public async Task Clear(string key)
        {
            await _cache.KeyDeleteAsync(key);
        }

        public void ClearAll()
        {
            var endPoints = _redisConnection.GetEndPoints(true);
            foreach (var endPoint in endPoints)
            {
                var server = _redisConnection.GetServer(endPoint);
                server.FlushAllDatabases();
            }
        }

        public T GetOrAdd<T>(string key, Func<T> action) where T : class
        {
            var result = _cache.StringGet(key);
            if (result.IsNull)
            {
                result = JsonSerializer.SerializeToUtf8Bytes(action());
                _cache.StringSet(key, result);
            }
            return JsonSerializer.Deserialize<T>(result);
        }

        public async Task<T> GetOrAddAsync<T>(string key, Func<Task<T>> action) where T : class
        {
            var result = await _cache.StringGetAsync(key);
            if (result.IsNull)
            {
                result = JsonSerializer.SerializeToUtf8Bytes(await action());
                await SetValueAsync(key, result);
            }
            return JsonSerializer.Deserialize<T>(result);
        }

        public async Task<string> GetValueAsync(string key)
        {
            return await _cache.StringGetAsync(key);
        }

        public async Task<bool> SetValueAsync(string key, string value)
        {
            return await _cache.StringSetAsync(key, value);
        }

        public bool DeleteByKey(string key)
        {
            return _cache.KeyDelete(key);
        }

        public TimeSpan? GetKeyTime(string key)
        {
            return _cache.KeyTimeToLive(key);
        }
        /// <summary>
        /// Önbellekte bulunan verilerin anahtar listesini getirir.
        /// </summary>
        /// <returns></returns>
        public List<RedisKey> GetAllKeys()
        {
            var endPoint = _redisConnection.GetEndPoints(true).First();
            var server = _redisConnection.GetServer(endPoint);
            return server.Keys(_cache.Database, pattern: "*").ToList();
        }
        public bool HSet(string key, string field, string value)
        {
            return _cache.HashSet(key,field,value);
        }

        public Dictionary<string, string> HGetAll(string key)
        {
            var hashEntries = _cache.HashGetAll(key);
            var result = new Dictionary<string, string>();
            foreach (var entry in hashEntries)
            {
                result.Add(entry.Name, entry.Value);
            }
            return result;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            GC.Collect();
        }
    }
}
