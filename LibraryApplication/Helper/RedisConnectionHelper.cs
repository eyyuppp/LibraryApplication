using StackExchange.Redis;

namespace LibraryApplication.Helper
{
    public static class RedisConnectionHelper
    {
        /// <summary>
        /// Redis'e bağlantı sağlar
        /// </summary>
        public static IConnectionMultiplexer Connection { get; } = ConnectionMultiplexer.Connect("localhost:6379");
    }
}
