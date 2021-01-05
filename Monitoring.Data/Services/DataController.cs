using Microsoft.EntityFrameworkCore;
using Monitoring.Data.Entities;
using Monitoring.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monitoring.Data.Services
{
    public class DataController : IDataController
    {
        private readonly MonitoringDbContext _context;

        public DataController(MonitoringDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateConfiguration(MonitoringConfiguration monitorConfig)
        {
            await _context.MonitoringConfiguration.AddAsync(monitorConfig);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> CreateReportAsync(MonitoringReport monitoringReport)
        {
            await _context.MonitoringReport.AddAsync(monitoringReport);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<MonitoringConfiguration> ReadConfiguration(Guid id)
         => await _context.MonitoringConfiguration.FindAsync(id);

        public async Task<MonitoringReport> GetLatestTask(Guid taskId, Guid configId, string taskType)
        => await _context.MonitoringReport.OrderByDescending(a => a.TimeStamp).FirstOrDefaultAsync();

    }
}