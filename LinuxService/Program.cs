using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;
using System.Threading.Tasks;


namespace LinuxService
{
    class Program
    {
        private const string _prefix = "ASPNETCORE_";
        private const string _appsettings = "appsettings.json";
        private const string _hostsettings = "hostsettings.json";

        static async Task Main(string[] args)
        {
            IHost host = new HostBuilder()
                .ConfigureHostConfiguration(configHost =>
                {
                    configHost.SetBasePath(Directory.GetCurrentDirectory());
                    configHost.AddJsonFile(_hostsettings, optional: true);
                    configHost.AddEnvironmentVariables(prefix: _prefix);
                    configHost.AddCommandLine(args);

                })
                 .ConfigureAppConfiguration((hostContext, configApp) =>
                 {
                     configApp.SetBasePath(Directory.GetCurrentDirectory());
                     configApp.AddJsonFile(_appsettings, true);
                     configApp.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", true);
                     configApp.AddEnvironmentVariables(prefix: _prefix);
                     configApp.AddCommandLine(args);
                 })

                 .ConfigureServices((hostContext, services) =>
                 {
                     services.AddLogging();

                     services.Configure<RedisConfiguration>(hostContext.Configuration.GetSection("Redis"));
                     services.AddSingleton<IRedisConnectionFactory, RedisConnectionFactory>();
                     services.AddSingleton<IHostedService, RedisService>();


                 })
                .ConfigureLogging((hostContext, configLogging) =>
                {
                    configLogging.AddSerilog(new LoggerConfiguration()
                              .ReadFrom.Configuration(hostContext.Configuration)
                              .CreateLogger());
                })
            .UseConsoleLifetime()
            .Build();



            await host.RunAsync();
        }
    }
}
