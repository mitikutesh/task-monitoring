using Microsoft.Extensions.Logging;
using Monitoring.Data.Interfaces;
using Monitoring.Infrastructure.Helpers;
using Monitoring.Infrastructure.Models;
using Monitoring.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monitoring.Service.Services
{
    public abstract class BaseTask : IBaseTask
    {
        protected readonly IDataController _dataCtr;
        protected readonly ILogger _logger;

        protected virtual TasksToDo task { get; set; }

        public BaseTask(IDataController dataCtr, ILogger<TasksToDo> logger)
        {
            _logger = logger;
            _dataCtr = dataCtr;
        }

        public abstract Task StartTask(TasksToDo task, string configID, string clientID, string guid);

        protected async Task<bool> IsDoTaskOk(TasksToDo task, string configId, string customerId)
        {
            if (!(await IsInitDataOk(task, configId, customerId)))
                return false;

            var latestTask = await _dataCtr.GetLatestTask(task.Id, configId.StringToGuid(), task.Type);
            if (latestTask?.TimeStamp == null)
                return true;

            var latestDate = latestTask.TimeStamp;
            var ckDate = DateTime.Now.AddMinutes(-(task.Interval.Minutes));
            if (DateTime.Compare(latestDate, ckDate) <= 0)
                return true;

            _logger.LogInformation($"{task.Type.ToUpper()} with Task ID: [{task.Id}] is uptodate.");
            return false;
        }
        public abstract Task<bool> IsInitDataOk(TasksToDo task, string configID, string clientID);

        //in a every given day must repeat similar time interval
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

        public bool IsWhiteListed(System.Collections.Generic.List<RunOn> runons)
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