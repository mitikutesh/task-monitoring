using Monitoring.Job.Interfaces;
using Monitoring.Job.Jobs;
using System;

namespace Monitoring.Jobs.Interfaces
{
    public interface ITaskFactory
    {
        IBaseTask GetTask(TaskEnum type, IServiceProvider _serviceProvider);
    }
}