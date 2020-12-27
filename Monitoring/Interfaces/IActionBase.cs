using System.Collections.Generic;
using System.Threading.Tasks;
using Monitoring.Infrastructure.Models;

namespace Monitoring.Interfaces
{
    public interface IActionBase
    {
        Task StartTask(TasksToDo task, string configID, string clientID, string guid);
        bool IsInitDataOk(TasksToDo task, string configID, string customerID);
        bool IsWhiteListed(List<RunOn> runons);
        virtual string CreateScheduleTime(Interval interval, List<RunOn> runons);
    }
}