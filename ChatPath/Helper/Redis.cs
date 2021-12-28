using StackExchange.Redis;

namespace ChatPath
{
    public static class Redis
    {
        public static ConnectionMultiplexer get()
        {
            return ConnectionMultiplexer.Connect(Startup.redisServer);
        }
    }
}
