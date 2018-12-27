using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LinuxService
{
    public class RedisService : IHostedService
    {
        private readonly IApplicationLifetime _appLifetime;
        private readonly ILogger<RedisService> _logger;
        private readonly IHostingEnvironment _environment;
        private readonly IConfiguration _configuration;
        private readonly IRedisConnectionFactory _redisConnectionFactory;

        public RedisService(
            IConfiguration configuration,
            IHostingEnvironment environment,
            ILogger<RedisService> logger,
            IApplicationLifetime appLifetime,
            IRedisConnectionFactory redisConnectionFactory
        )
        {
            _configuration = configuration;
            _logger = logger;
            _appLifetime = appLifetime;
            _environment = environment;
            _redisConnectionFactory = redisConnectionFactory;
        }


        public Task StartAsync(CancellationToken cancellationToken)
        {
            this._appLifetime.ApplicationStarted.Register(Subscribe);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("StopAsync method called.");
            return Task.CompletedTask;
        }

        private void Subscribe()
        {
            _logger.LogInformation("Subscribe method called.");
            Console.WriteLine("Subscribe chanel-2");

            var sub = _redisConnectionFactory.Connection().GetSubscriber();
            sub.Subscribe("chanel-2", (channel, message) =>
            {
                Console.WriteLine(message);
                _logger.LogDebug($"Subscribe method : {message}");
            });
        }
    }
}
