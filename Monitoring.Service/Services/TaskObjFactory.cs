using Microsoft.Extensions.DependencyInjection;
using Monitoring.Service.Interfaces;
using Monitoring.Service.JobHelper;
using System;

namespace Monitoring.Service.Services
{
    public class TaskObjFactory : ITaskObjFactory
    {
        public TaskObjFactory()
        {
        }

        public TaskObjFactory(IServiceProvider serviceProvider)
        {
            //_serviceProv = serviceProvider;
        }

        public IBaseTask GetTask(TaskEnum type, IServiceProvider _serviceProvider)
        {
            switch (type)
            {
                case TaskEnum.PingFactory:
                    return (IPingFactory)_serviceProvider.GetRequiredService(typeof(IPingFactory));

                case TaskEnum.SqlFactory:
                    return (ISqlFactory)_serviceProvider.GetRequiredService(typeof(ISqlFactory));

                default:
                    return null;
            }
        }
    }
}
