﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Monitoring.Data;
using Monitoring.Data.Interfaces;
using Monitoring.Data.Services;
using Monitoring.Infrastructure.Models;
using Monitoring.Service.Interfaces;
using Monitoring.Service.Jobs;
using Monitoring.Service.Services;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using static Microsoft.Extensions.Hosting.EnvironmentName;

namespace Monitoring
{
    internal class Program
    {
        internal static async System.Threading.Tasks.Task Main(string[] args)
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
                    services.Configure<MonitorSettings>(hostContext.Configuration.GetSection(MonitorSettings.MonitorSetting));
                    services.AddHostedService<MonitoringService>();
                    services.AddDbContext<MonitoringDbContext>(options =>
                    {
                        var t = hostContext.Configuration.GetSection("MonitorSettings:ConnectionString");
                        options.UseSqlite(hostContext.Configuration.GetSection("MonitorSettings:ConnectionString")?.Value ?? throw new Exception("Null connection string."));
                    });
                    services.AddScoped<IDataController, DataController>();
                    services.AddScoped<IPingFactory, PingFactory>();
                    services.AddScoped<ISqlFactory, SqlFactory>();
                    services.AddScoped<ITaskObjFactory, TaskObjFactory>();

                })
                .ConfigureAppConfiguration((hostingContext, configApp) =>
                {
                    configApp.AddJsonFile("hostSettings.json", optional: false, reloadOnChange: true);
                });
    }
}