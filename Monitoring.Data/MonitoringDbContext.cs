using System;
using Microsoft.EntityFrameworkCore;
using Monitoring.Data.Entities;

namespace Monitoring.Data
{
    public class MonitoringDbContext : DbContext
    {
        public virtual DbSet<MonitoringConfiguration> MonitoringConfiguration { get; set; }
        public virtual DbSet<MonitoringClient> MonitoringClient { get; set; }
        public virtual DbSet<MonitoringLog> MonitoringLog { get; set; }
        public virtual DbSet<MonitoringReport> MonitoringReport { get; set; }
        
        public MonitoringDbContext()
        {
        }

        public MonitoringDbContext(DbContextOptions<MonitoringDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MonitoringConfiguration>(e =>
            {
                e.HasKey(e => e.Id);
            });
            modelBuilder.Entity<MonitoringClient>(e =>
            {
                e.HasKey(e => e.Id);
            });
            modelBuilder.Entity<MonitoringLog>(e =>
            {
                e.HasKey(e => e.Id);
            });
            modelBuilder.Entity<MonitoringReport>(e =>
            {
                e.HasKey(e => e.Id);
            });
        }
    }
}