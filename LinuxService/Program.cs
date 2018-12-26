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
        static async Task Main(string[] args)
        {
            IHost host = new HostBuilder()
                .ConfigureHostConfiguration(configHost =>
                {
                    configHost.SetBasePath(Directory.GetCurrentDirectory());
                    configHost.AddEnvironmentVariables(prefix: "ASPNETCORE_");
                })
                 .ConfigureAppConfiguration((hostContext, configApp) =>
                 {
                     configApp.SetBasePath(Directory.GetCurrentDirectory());
                     configApp.AddEnvironmentVariables(prefix: "ASPNETCORE_");
                     configApp.AddJsonFile($"appsettings.json", true);
                     configApp.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", true);
                 })

                 .ConfigureServices((hostContext, services) =>
                 {
                     services.AddLogging();
                     services.AddSingleton<IRedisConnectorHelper, RedisConnectorHelper>();
                     services.AddSingleton<IHostedService, RedisService>();
                 })

                .ConfigureLogging((hostContext, configLogging) =>
                {
                    configLogging.AddSerilog(new LoggerConfiguration()
                              .ReadFrom.Configuration(hostContext.Configuration)
                              .CreateLogger());
                })

            .Build();

            await host.RunAsync();
        }
    }
}
