using StackExchange.Redis;
using System.Text.Json;

namespace DataAccess.Redis
{
    public class CacheRepository : ICacheRepository
    {
        private readonly IConnectionMultiplexer _redisConnection =ConnectionMultiplexer.Connect("localhost:6379");
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
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            GC.Collect();
        }

    }
}
