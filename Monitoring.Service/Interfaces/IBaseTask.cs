using Monitoring.Infrastructure.Models;
using System.Threading.Tasks;

namespace Monitoring.Service.Interfaces
{
    public interface IBaseTask
    {
        Task StartTask(TasksToDo task, string configID, string clientID, string guid);

        Task<bool> IsInitDataOk(TasksToDo task, string configID, string clientID);
    }
}