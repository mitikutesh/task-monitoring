namespace Monitoring.Infrastructure.Models
{
    public class MonitorSettings
    {
        public const string MonitorSetting = "MonitorSettings";
        public string ClientId { get; set; }
        public string ConfigId { get; set; }
        public string OutputFilePath { get; set; }
        public string ConnectionString { get; set; }
        public string Schedule { get; set; } //"MINUTES HOURS DAYS MONTHS DAYS-OF-WEEK"
    }
}