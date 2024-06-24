using DataAccess.Redis;
using StackExchange.Redis;

namespace SignalR.StartupConfiguration
{
    public static class RedisConfurationExtension
    {
        public static void AddRedisConnection(this IServiceCollection services, IConfiguration configuration)
        {
            string uri = configuration.GetConnectionString("Redis");
            var multiplexer = ConnectionMultiplexer.Connect(uri);
            services.AddSingleton<IConnectionMultiplexer>(multiplexer);
            services.AddSingleton<ICacheRepository, CacheRepository>();
        }
    }
}
