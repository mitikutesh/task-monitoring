using Microsoft.Extensions.Logging;
using Monitoring.Data.Interfaces;
using Monitoring.Infrastructure.Models;
using NCrontab;
using System;
using System.Threading.Tasks;

namespace Monitoring.Service.Services
{
    public abstract class ScheduledProcessor : ActionBase
    {
        private CrontabSchedule _schedule;
        private DateTime _nextRun;
        private DateTime _startDate;
        ILogger<ScheduledProcessor> _logger;

        protected ScheduledProcessor(IDataController dataCtr, ILogger<ScheduledProcessor> logger) : base(dataCtr, logger)
        {
            _startDate = DateTime.Now;
            _logger = logger;
        }

        public abstract Task Process(TasksToDo task, string configID, string customerId, string guid);

        public override async Task StartTask(TasksToDo task, string configID, string customerId, string guid)
        {
            await Process(task, configID, customerId, guid);
            await Task.CompletedTask;
        }
    }
}