using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Monitoring.Services
{
    public static class ServiceBaseLiveTimeHostExtension
    {
        public static IHostBuilder UseServiceBaseLifetime(this IHostBuilder hostBuilder)
        {
            return hostBuilder
                    .UseEnvironment(EnvironmentName.Development)
                    .ConfigureServices((hostContext, services) =>
                    {
                        services.AddSingleton<IHostedService, HostedMonitoringServiceBase>();
                      //TODO configure services
                       
                    })
                    .ConfigureAppConfiguration((hostingContext, configApp) =>
                    {
                        configApp.AddJsonFile("hostSettings.json", optional: true, reloadOnChange: true);
                    })
                    .ConfigureLogging((hostContext, configLogging) =>
                    {
                        configLogging.ClearProviders();
                        configLogging.AddConfiguration(hostContext.Configuration.GetSection("Logging"));
                        configLogging.AddConsole();
                    });
        }

        public static Task RunAsServiceAsync(this IHostBuilder
        hostBuilder, CancellationToken cancellationToken = default)
        {
            return
            hostBuilder.UseServiceBaseLifetime().Build()
           .RunAsync(cancellationToken);
        }
    }
}