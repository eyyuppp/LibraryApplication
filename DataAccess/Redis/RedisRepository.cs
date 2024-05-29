using StackExchange.Redis;
using IDatabase = StackExchange.Redis.IDatabase;

namespace DataAccess.Redis
{
    public class RedisRepository : IRedisRepository
    {
        public IDatabase RedisDatabase { get; set; }

        private IRedisConnection _redisConnectionObj;
        public IRedisConnection RedisConnectionObj
        {
            get
            {
                if (_redisConnectionObj == null)
                {
                    _redisConnectionObj = Activator.CreateInstance<RedisConnection>();
                }
                return _redisConnectionObj;
            }
            set
            {
                _redisConnectionObj = value;
            }
        }

        #region Singleton Section

        public RedisRepository(RedisDatabaseEnum redisDatabaseEnum)
        {
            RedisDatabase = RedisConnectionObj.GetDatabase(redisDatabaseEnum);
        }

        #endregion

        /// <summary>
        /// Redis önbellekte tutulan veriyi getirir.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Get(string key)
        {
            return RedisDatabase.StringGet(key);
        }

        /// <summary>
        /// Önbellekte tutulan veriyi siler.
        /// </summary>
        /// <param name="key"></param>
        public bool DeleteByKey(string key)
        {
            return RedisDatabase.KeyDelete(key);
        }

        /// <summary>
        /// Önbellekte tutulan veriyi siler.
        /// </summary>
        /// <param name="key"></param>
        public Task<bool> DeleteByKeyAsync(string key)
        {
            return RedisDatabase.KeyDeleteAsync(key);
        }

        /// <summary>
        /// Önbellekte tutulan verileri key benzerliğine göre siler.
        /// </summary>
        /// <param name="key"></param>
        public bool DeleteByKeyLike(string key)
        {
            List<RedisKey> keys = GetAllKeys();
            if (keys.Any(p => p.ToString().Contains(key)))
            {
                keys.Where(p => p.ToString().Contains(key)).ToList().ForEach(p => DeleteByKey(p));
                return true;
            }

            return false;
        }

        /// <summary>
        /// Önbellekte tutulan verileri ön ekine göre siler.
        /// </summary>
        /// <param name="key"></param>
        public bool DeleteByPrefix(string prefix)
        {
            List<RedisKey> keys = GetAllKeys();
            if (keys.Any(p => p.ToString().StartsWith(prefix)))
            {
                keys.Where(p => p.ToString().StartsWith(prefix)).ToList().ForEach(p => DeleteByKey(p));
                return true;
            }

            return false;
        }

        /// <summary>
        /// Tüm listeyi temizler
        /// </summary>
        public bool Clear()
        {
            var list = GetAllKeys();
            list.ForEach(p => DeleteByKey(p));
            return list.Any();
        }

        /// <summary>
        /// Async olarak tutulan veriyi getirir.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expireTime"></param>
        public Task<bool> SetAsync(string key, string value, TimeSpan? expireTime = null)
        {
            if (expireTime > TimeSpan.MinValue)
                return RedisDatabase.StringSetAsync(key, value, expireTime);
            return RedisDatabase.StringSetAsync(key, value);
        }

        /// <summary>
        /// Önbellekte veriyi, verilmişse istenilen süre kadar tutar
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expireTime"></param>
        public bool Set(string key, string value, TimeSpan? expireTime = null)
        {
            if (expireTime > TimeSpan.MinValue)
                return RedisDatabase.StringSet(key, value, expireTime);
            return RedisDatabase.StringSet(key, value);
        }

       
        /// <summary>
        /// Anahtara göre var olup olmadığını döner
        /// </summary>
        /// <param name="key"></param>
        public bool Exists(string key)
        {
            List<RedisKey> keys = GetAllKeys();
            return keys.Contains(key);
        }

        /// <summary>
        /// Anahtara göre var olun önbelleği döner
        /// </summary>
        /// <param name="key"></param>
        public bool GetIfExists(string key, out string obj)
        {
            obj = null;

            if (this.Exists(key))
            {
                obj = this.RedisDatabase.StringGet(key);
            }

            return (obj != null);
        }

       

        /// <summary>
        /// Anahtara göre var olun önbelleği döner
        /// </summary>
        /// <param name="key"></param>
        public bool GetIfExistsObj(string key, out object obj)
        {
            obj = null;

            if (this.Exists(key))
            {
                obj = this.Get(key);
                if (obj == null)
                {
                    this.DeleteByKey(key);
                    return false;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Anahtarın benzerliğine göre var olup olmadığını döner
        /// </summary>
        /// <param name="key"></param>
        public bool ExistsLike(string key)
        {
            List<RedisKey> keys = GetAllKeysByLike(key);
            return keys.Any();
        }

        /// <summary>
        /// Anahtarın benzerliğine göre var olup olmadığını döner
        /// </summary>
        /// <param name="key"></param>
        public bool ExistsPrefixAndLike(string prefix, List<string> subTextList)
        {
            List<RedisKey> keys = GetAllKeys();
            var list = keys.Where(p => p.ToString().StartsWith(prefix)).Select(p => p.ToString().Replace(prefix, ""));
            return list.Any(p => subTextList.Contains(p));
        }

        /// <summary>
        /// Önbellekte bulunan verilerin anahtar listesini getirir.
        /// </summary>
        /// <returns></returns>
        public List<RedisKey> GetAllKeys()
        {
            return RedisConnectionObj.GetConnection().GetServer(RedisConnection.Domain).Keys(RedisDatabase.Database, pattern: "*").ToList();
        }


        /// <summary>
        /// Önbellekte bulunan verilerin benzerliğine göre anahtar listesini getirir.
        /// </summary>
        /// <returns></returns>
        public List<RedisKey> GetAllKeysByLike(string key)
        {
            List<RedisKey> keys = RedisConnectionObj.GetConnection().GetServer(RedisConnection.Domain).Keys(pattern: "*").ToList();
            return keys.Where(p => p.ToString().Contains(key)).ToList();
        }

        /// <summary>
        /// Önbellekte bulunan verilerin kalan zamanlarını getirir.
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, TimeSpan?> GetAllKeyTime()
        {
            Dictionary<string, TimeSpan?> keyExpireTime = new Dictionary<string, TimeSpan?>();

            List<RedisKey> keys = GetAllKeys();
            foreach (var key in keys)
            {
                keyExpireTime.Add(key, RedisDatabase.KeyTimeToLive(key));
            }

            return keyExpireTime;
        }

        /// <summary>
        /// İstenilen keyin kalan zamanını dönderir.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TimeSpan? GetKeyTime(string key)
        {
            return RedisDatabase.KeyTimeToLive(key);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            GC.Collect();
        }
    }

}