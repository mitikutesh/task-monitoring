using System;
using System.Threading.Tasks;
using Monitoring.Data.Entities;
using Monitoring.Data.Interfaces;

namespace Monitoring.Data.Services
{
    public class DataController :IDataController
    {
        private readonly MonitoringDbContext _context;

        public DataController(MonitoringDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateConfiguration(MonitoringConfiguration monitorConfig)
        {
            await  _context.MonitoringConfiguration.AddAsync(monitorConfig);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> CreateReportAsync(MonitoringReport monitoringReport)
        {
            await  _context.MonitoringReport.AddAsync(monitoringReport);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<MonitoringConfiguration> ReadConfiguration(Guid id)
        {
           return await _context.MonitoringConfiguration.FindAsync(id);
        }
    }
}