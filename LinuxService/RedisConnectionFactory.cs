using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System;

namespace LinuxService
{
    public interface IRedisConnectionFactory
    {
        ConnectionMultiplexer Connection();
    }

    public class RedisConnectionFactory : IRedisConnectionFactory
    {
        private Lazy<ConnectionMultiplexer> _connection;

        private readonly RedisConfiguration _redisConfiguration;

        public RedisConnectionFactory(IOptions<RedisConfiguration> redis)
        {
            _redisConfiguration = redis.Value;
            _connection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(_redisConfiguration.Host));
        }

        public ConnectionMultiplexer Connection()
        {
            return _connection.Value;
        }
    }
}
