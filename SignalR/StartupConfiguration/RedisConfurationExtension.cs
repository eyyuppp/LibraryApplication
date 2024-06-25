using DataAccess.Redis;

namespace SignalR.StartupConfiguration
{
    public static class RedisConfurationExtension
    {
        public static void AddRedisConnection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ICacheRepository, CacheRepository>();
        }
    }
}
