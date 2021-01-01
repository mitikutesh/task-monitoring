using Monitoring.Service.JobHelper;
using System;

namespace Monitoring.Service.Interfaces
{
    public interface ITaskObjFactory
    {
        IBaseTask GetTask(TaskEnum type, IServiceProvider _serviceProvider);
    }
}