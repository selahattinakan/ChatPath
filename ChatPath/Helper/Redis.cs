using StackExchange.Redis;

namespace ChatPath
{
    public static class Redis
    {
        public const string redisServer = "127.0.0.1:6379";
        public static ConnectionMultiplexer get()
        {
            return ConnectionMultiplexer.Connect(redisServer);
        }
    }
}
