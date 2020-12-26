using System;
using System.ComponentModel.DataAnnotations;

namespace Monitoring.Data.Entities
{
    public class MonitoringReport
    {
        [Key]
        public Guid Id { get; set; }
        public Guid ConfigId { get; set; }
        public Guid ClientId { get; set; }
        public Guid TaskId { get; set; }
        public DateTime TimeStamp { get; set; }
        public string TaskType { get; set; }
        public string Result { get; set; }
        public int Level { get; set; }
        public virtual MonitoringConfiguration MonitoringConfiguration { get; set; }
        public virtual MonitoringClient MonitoringClient { get; set; }
    }
}