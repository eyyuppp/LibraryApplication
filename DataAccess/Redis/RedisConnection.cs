using StackExchange.Redis;

namespace DataAccess.Redis
{
    public class RedisConnection : IRedisConnection
    {
        public static readonly string Domain = Environment.GetEnvironmentVariable("REDIS_URL");

       
        public ConnectionMultiplexer GetConnection()
        {
            var connection = ConnectionMultiplexer.Connect(GetConfiguration());
            return connection;
        }

        public ConfigurationOptions GetConfiguration()
        {
            ConfigurationOptions options = ConfigurationOptions.Parse(Domain);
            options.AbortOnConnectFail = false;
            return options;
        }
    }
}
