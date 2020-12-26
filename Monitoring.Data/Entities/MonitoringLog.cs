using System;
using System.ComponentModel.DataAnnotations;

namespace Monitoring.Data.Entities
{
    public class MonitoringLog
    {
        [Key]
        public int Id { get; set; }
        public DateTime? Timestamp { get; set; }
        public string Level { get; set; }
        public string Logger { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public string Stacktrace { get; set; }
    }
}