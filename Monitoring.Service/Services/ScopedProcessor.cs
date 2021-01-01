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
        private readonly IServiceScopeFactory _serviceScopeFactory;
        protected  IDataController _dataController;

        public ScopedProcessor(IServiceScopeFactory serviceScopeFactory, ILogger<ScopedProcessor> logger, IHostApplicationLifetime appLifetime)
            : base(logger, appLifetime)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }
        protected override async Task ProcessAsync()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                _dataController = scope.ServiceProvider.GetRequiredService<IDataController>();
                await ProcessInScopeAsync(scope.ServiceProvider);
            }
        }
        public abstract Task ProcessInScopeAsync(IServiceProvider serviceProvider);

    }
}