using StackExchange.Redis;

namespace DataAccess.Redis
{
    public interface IRedisConnection
    {
        public ConnectionMultiplexer GetConnection();
        public ConfigurationOptions GetConfiguration();
    }
}
