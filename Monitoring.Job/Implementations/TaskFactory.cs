using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitoring.Job.Implementations
{
    class TaskFactory : ITaskFactory
    {
        public TaskFactory()
        {
        }

        public TaskFactory(IServiceProvider serviceProvider)
        {
            //_serviceProv = serviceProvider;
        }

        public ITaskBase GetTask(EnumTask type, IServiceProvider _serviceProvider)
        {
            switch (type)
            {
                case EnumTask.PingFactory:
                    return (IPingFactory)_serviceProvider.GetRequiredService(typeof(IPingFactory));

                case EnumTask.SqlFactory:
                    return (ISqlFactory)_serviceProvider.GetRequiredService(typeof(ISqlFactory));

                default:
                    return null;
            }
        }
    }
}
