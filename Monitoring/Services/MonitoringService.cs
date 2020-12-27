using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Monitoring.Data.Interfaces;

namespace Monitoring.Services
{
    public class MonitoringService : ScopedProcessor
    {
        private readonly IDataController _dataCtrl;
        
        public MonitoringService(IDataController dataCtrl, IServiceScopeFactory serviceScopeFactory, ILogger<ScopedProcessor> logger, IHostApplicationLifetime appLifetime) : base(serviceScopeFactory, logger, appLifetime)
        {
            _dataCtrl = dataCtrl;
        }

        //scoped call
        public override async Task ProcessInScopeAsync(IServiceProvider serviceProvider)
        {
            //TODO run tasks based on configuration
            throw new NotImplementedException();
        }
    }
}