using StackExchange.Redis;

namespace LinuxService
{
    public interface IRedisConnectionFactory
    {
        ConnectionMultiplexer Connection();
    }
}
