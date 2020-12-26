using System;
using System.ComponentModel.DataAnnotations;

namespace Monitoring.Data.Entities
{
    public class MonitoringConfiguration
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Configuration { get; set; }
        public DateTime LastModified { get; set; }
    }
}