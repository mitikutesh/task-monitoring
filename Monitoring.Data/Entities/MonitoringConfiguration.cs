using System;

namespace Monitoring.Data.Entities
{
    public class MonitoringConfiguration
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Configuration { get; set; }
        public DateTime LastModified { get; set; }
    }
}