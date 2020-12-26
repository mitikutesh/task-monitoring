using System;
using System.Threading.Tasks;
using Monitoring.Data.Entities;

namespace Monitoring.Data.Interfaces
{
    public interface IDataController
    {
        Task<bool> CreateConfiguration(MonitoringConfiguration monitorConfig);
        Task<bool> CreateReportAsync(MonitoringReport monitoringReport);
        Task<MonitoringConfiguration> ReadConfiguration(Guid id);
    }
}