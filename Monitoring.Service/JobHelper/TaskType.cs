using System.Collections.Generic;

namespace Monitoring.Service.JobHelper
{
    public class TaskType
    {
        public static Dictionary<string, TaskEnum> DicType { get; } =
            new Dictionary<string, TaskEnum>()
            {
                {"PING", TaskEnum.PingFactory },
                {"SQL", TaskEnum.SqlFactory},
                {"CERTIFICATE", TaskEnum.CertificateValidator}
            };
    }
}