using Monitoring.Infrastructure.Models;
using System.Collections.Generic;

namespace Monitoring.Interfaces
{
    public interface IActionBase
    {

        System.Threading.Tasks.Task StartTask(TasksToDo task, string configID, string clientID, string guid);
        bool IsInitDataOk(TasksToDo task, string configID, string customerID);
        bool IsWhiteListed(List<RunOn> runons);
        string CreateScheduleTime(Interval interval, List<RunOn> runons);
    }
}