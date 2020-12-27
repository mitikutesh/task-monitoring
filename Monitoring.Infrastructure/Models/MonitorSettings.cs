namespace Monitoring.Infrastructure.Models
{
    public record MonitorSettings(string ClientId, string ConfigId, string OutPutFilePath, string DbString,
        string Schedule); ////"MINUTES HOURS DAYS MONTHS DAYS-OF-WEEK"

}