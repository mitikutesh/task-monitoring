using Monitoring.Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Monitoring.Service.Interfaces
{
    public interface IActionBase
    {
        Task StartTask(TasksToDo task, string configID, string clientID, string guid);
        Task<bool> IsInitDataOk(TasksToDo task, string configID, string customerID);
        bool IsWhiteListed(List<RunOn> runons);
        string CreateScheduleTime(Interval interval, List<RunOn> runons);
    }
}