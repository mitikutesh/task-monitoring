using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Monitoring.Services;
using static Microsoft.Extensions.Hosting.EnvironmentName;

namespace Monitoring
{
    class Program
    {
       
        static async  Task Main(string[] args)
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
                    else
                    {
                        //TODO
                    }
                }

                //using generic host
                var host = new HostBuilder()
                    .ConfigureServices((_, services) =>
                    {
                        services.AddHostedService<HostedMonitoringServiceBase>();
                    });
                
                host.UseEnvironment(isService ? Production : Development);

                
                if (isService)
                {
                    await host.RunAsServiceAsync();
                }
                else
                {
                    host.UseServiceBaseLifetime()
                        .ConfigureHostConfiguration(config =>
                        {
                            config.AddCommandLine(args);
                            if (args != null)
                            {
                                config.AddCommandLine(args);
                            }
                        });
                    await host.RunConsoleAsync();
                }

            }
            catch (Exception ex)
            {
                File.WriteAllText(@"D:\Temp\MitikuServiceBug.txt", ex.ToString());
                //TODO log error
            }
            finally
            {
                //clearn up
            }

        }

        // static IHostBuilder CreateHostBuilder(string[] args) =>
        //     Host.CreateDefaultBuilder(args)
        //         .ConfigureServices((_, services) =>
        //             services.AddHostedService<HostedMonitoringService>());

        // .ConfigureHostConfiguration(configHost =>
        // {
        //     configHost.SetBasePath(Directory.GetCurrentDirectory());
        //     configHost.AddJsonFile("hostsettings.json", optional: true);
        //     configHost.AddEnvironmentVariables(prefix: "PREFIX_");
        //     configHost.AddCommandLine(args);
        // });
    }
}
