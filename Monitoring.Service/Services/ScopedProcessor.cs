using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Monitoring.Data.Interfaces;
using System;
using System.Threading.Tasks;

namespace Monitoring.Service.Services
{
    public abstract class ScopedProcessor : MonitoringBackgroundService
    {
        //private readonly IServiceProvider _services;
        public IServiceProvider Services { get; }
        protected  IDataController _dataController;

        public ScopedProcessor(IServiceProvider services, ILogger<ScopedProcessor> logger, IHostApplicationLifetime appLifetime)
            : base(logger, appLifetime)
        {
            Services = services;
        }
        protected override async Task ProcessAsync()
        {
            using (var scope = Services.CreateScope())
            {
                _dataController = scope.ServiceProvider.GetRequiredService<IDataController>();
                await ProcessInScopeAsync(scope.ServiceProvider);
            }
        }
        public abstract Task ProcessInScopeAsync(IServiceProvider serviceProvider);

    }
}