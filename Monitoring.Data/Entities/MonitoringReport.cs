using System;

namespace Monitoring.Data.Entities
{
    public class MonitoringReport
    {
        public Guid Id { get; set; }
        public Guid ConfigId { get; set; }
        public Guid ClientrId { get; set; }
        public Guid TaskId { get; set; }
        public DateTime TimeStamp { get; set; }
        public string TaskType { get; set; }
        public string Result { get; set; }
        public int Level { get; set; }
    }
}