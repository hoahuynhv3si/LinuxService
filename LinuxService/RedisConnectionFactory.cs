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
        }

        public ConnectionMultiplexer Connection()
        {
            if(_connection == null)
            {
                _connection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(_redisConfiguration.Host));
            }

            return _connection.Value;
        }
    }
}
