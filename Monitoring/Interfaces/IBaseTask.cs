using System;
using System.Threading.Tasks;
using Monitoring.Infrastructure.Models;

namespace Monitoring.Task.Interfaces
{
    public interface IBaseTask
    {
        Task<bool> StartTask(TasksToDo task, string configID, string clientID, string guid);

        Task<bool> IsInitDataOk(TasksToDo task, string configID, string clientID);
    }
}