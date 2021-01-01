namespace Monitoring.Infrastructure.Models
{
    //public record MonitorSettings(string ClientId, string ConfigId, string OutPutFilePath, string ConnectionString,
    //    string Schedule); ////"MINUTES HOURS DAYS MONTHS DAYS-OF-WEEK"

    public class MonitorSettings
    {
        public string ClientId { get; set; }
        public string ConfigId { get; set; }
        public string OutputFilePath { get; set; }
        public string ConnectionString { get; set; }
        public string Schedule { get; set; } //"MINUTES HOURS DAYS MONTHS DAYS-OF-WEEK"
    }
}