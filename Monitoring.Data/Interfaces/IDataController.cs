using Monitoring.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Monitoring.Data.Interfaces
{
    public interface IDataController
    {
        Task<bool> CreateConfiguration(MonitoringConfiguration monitorConfig);
        Task<bool> CreateReportAsync(MonitoringReport monitoringReport);
        Task<MonitoringConfiguration> ReadConfiguration(Guid id);
        Task<MonitoringReport> GetLatestTask(Guid taskId, Guid configId, string taskType);
    }
}