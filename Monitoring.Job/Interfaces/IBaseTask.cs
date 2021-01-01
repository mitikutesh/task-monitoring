using Monitoring.Infrastructure.Models;
using System.Threading.Tasks;

namespace Monitoring.Job.Interfaces
{
    public interface IBaseTask
    {
        Task<bool> StartTask(TasksToDo task, string configID, string clientID, string guid);

        Task<bool> IsInitDataOk(TasksToDo task, string configID, string clientID);
    }
}