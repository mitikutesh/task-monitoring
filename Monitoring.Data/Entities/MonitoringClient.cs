using System;
using System.ComponentModel.DataAnnotations;

namespace Monitoring.Data.Entities
{
    public class MonitoringClient
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}