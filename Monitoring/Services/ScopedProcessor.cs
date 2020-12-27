using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Monitoring.Services
{
    public abstract class ScopedProcessor : MonitoringBackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        
        public ScopedProcessor(IServiceScopeFactory serviceScopeFactory,ILogger<ScopedProcessor> logger, IHostApplicationLifetime appLifetime) 
            : base(logger, appLifetime)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }
        protected override async Task ProcessAsync()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                await ProcessInScopeAsync(scope.ServiceProvider);
            }
        }
        public abstract Task ProcessInScopeAsync(IServiceProvider serviceProvider);
        
    }
}