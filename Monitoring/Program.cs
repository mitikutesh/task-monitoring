using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Monitoring.Data;
using Monitoring.Data.Interfaces;
using Monitoring.Data.Services;
using Monitoring.Services;
using static Microsoft.Extensions.Hosting.EnvironmentName;

namespace Monitoring
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            try
            {
                var isService = !(Debugger.IsAttached || args.Contains("--console"));
                if (isService)
                {
                    var processModule = Process.GetCurrentProcess().MainModule;
                    if (processModule != null)
                    {
                        var pathToExe = processModule.FileName;
                        var pathToContentRoot = Path.GetDirectoryName(pathToExe);
                        Directory.SetCurrentDirectory(pathToContentRoot);
                    }
                }

                //using generic host
                var host = CreateHostBuilder(args);
                host.UseEnvironment(isService ? Production : Development);


                if (isService)
                {
                    await host.Build().RunAsync();
                }
                else
                {
                    host.ConfigureHostConfiguration(config =>
                        {
                            config.AddCommandLine(args);
                            if (args != null) config.AddCommandLine(args);
                        });
                    await host.RunConsoleAsync();
                }
            }
            catch (Exception ex)
            {
                await File.WriteAllTextAsync(@"D:\Temp\MitikuServiceBug.txt", ex.ToString());
                //TODO log error
            }
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<MonitoringBackgroundService>();
                    
                    services.AddSingleton<IDataController, DataController>();
                    services.AddDbContext<MonitoringDbContext>(options =>
                    {
                        var t = hostContext.Configuration.GetSection("MonitorSettings:ConnectionString");
                        options.UseSqlite(hostContext.Configuration.GetSection("MonitorSettings:ConnectionString")?.Value ?? throw new Exception("Null connection string."));
                    });
                })
                .ConfigureHostConfiguration(configHost =>
                {
                    configHost.SetBasePath(Directory.GetCurrentDirectory());
                    configHost.AddJsonFile("hostsettings.json", optional: true, true);
                    configHost.AddEnvironmentVariables(prefix: "PREFIX_");
                })
                .ConfigureLogging((hostContext, configLogging) =>
                {
                    configLogging.ClearProviders();
                    configLogging.AddConfiguration(hostContext.Configuration.GetSection("Logging"));
                    configLogging.AddConsole();
                });
    }
}