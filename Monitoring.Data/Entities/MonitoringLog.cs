using System;

namespace Monitoring.Data.Entities
{
    public class MonitoringLog
    {
        public int Id { get; set; }
        public DateTime? Timestamp { get; set; }
        public string Level { get; set; }
        public string Logger { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public string Stacktrace { get; set; }
    }
}