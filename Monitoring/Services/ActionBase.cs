using Microsoft.Extensions.Logging;
using Monitoring.Data.Interfaces;
using Monitoring.Infrastructure.Models;
using Monitoring.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monitoring.Services
{
    public abstract class ActionBase : IActionBase
    {
        protected readonly IDataController _dataCtr;
        protected readonly ILogger _logger;

        public ActionBase(IDataController dataCtr, ILogger<ActionBase> logger)
        {
            _logger = logger;
            _dataCtr = dataCtr;
        }
        public abstract System.Threading.Tasks.Task StartTask(TasksToDo task, string configID, string customerId, string guid);
        public abstract bool IsInitDataOk(TasksToDo task, string configID, string customerID);

        protected async Task<bool> IsDoTaskOk(TasksToDo task, string configId, string customerId)
        {
            if (!IsInitDataOk(task, configId, customerId))
                return await System.Threading.Tasks.Task.FromResult(false);

            var latestTask = await _dataCtr.GetLatestTask(task.Id, new Guid(configId), task.Type);
            if (latestTask?.TimeStamp == null)
                return await System.Threading.Tasks.Task.FromResult(true); ;

            var latestDate = latestTask.TimeStamp;
            var ckDate = DateTime.Now.AddMinutes(-(task.Interval.Minutes));
            if (DateTime.Compare(latestDate, ckDate) <= 0)
                return await System.Threading.Tasks.Task.FromResult(true);

            _logger.LogInformation($"{task.Type.ToUpper()} with Task ID: [{task.Id}] is uptodate.");
            return await System.Threading.Tasks.Task.FromResult(false); ;
        }

        public virtual string CreateScheduleTime(Interval interval, List<RunOn> runons)
        {
            if (IsWhiteListed(runons))
            {
                return $"*/{interval.Minutes} */{interval.Hours} */{interval.Days} * *";
            }
            else
            {
                _logger.LogInformation("Task will not run because of white listing filter.");
                return null;
            }

        }
        public bool IsWhiteListed(List<RunOn> runons)
        {
            DateTime now = DateTime.Now;
            if (!runons.Any())
                return true;

            List<string> runonDays = runons.Select(a => a.Day).ToList();

            if (runonDays.Select(a => a.ToUpper()).Contains(now.DayOfWeek.ToString().ToUpper())) //day check
            {
                //time check
                foreach (var timeR in runons.Where(a => a.Day.Equals(now.DayOfWeek.ToString(), StringComparison.OrdinalIgnoreCase)).ToList())
                {
                    var start = DateTime.Parse(timeR.From, System.Globalization.CultureInfo.CurrentCulture);
                    var end = DateTime.Parse(timeR.To, System.Globalization.CultureInfo.CurrentCulture);

                    if (start <= end)
                    {
                        return start <= now && now <= end;
                    }
                    return !(end <= now && now <= start);

                }
            }
            return false;
        }

    }
}