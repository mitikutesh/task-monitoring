using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitoring.Job.Implementations
{
    public class CertValidator : ScheduledProcessor, ICertValidator
    {
        ILogger<CertValidator> _logger;

        public CertValidator(IDataController dataCtr, ILogger<CertValidator> logger) : base(dataCtr, logger)
        {
            _logger = logger;
        }


        public override bool IsInitDataOk(ToDoTask task, string configID, string customerID)
        {
            bool IsValid(string config)
            {
                return Guid.TryParse(config, out Guid guid);
            }

            if (string.IsNullOrWhiteSpace(task.Hostname) || string.IsNullOrWhiteSpace(configID) || string.IsNullOrWhiteSpace(customerID) || !task.Hostname.StartsWith("https://"))
            {
                return false;
            }
            if (IsValid(configID) && IsValid(customerID))
                return true;

            return false;
        }

        public override async Task Process(ToDoTask task, string configID, string customerId, string guid)
        {
            if (!IsDoTaskOk(task, configID, customerId))
                return;

            var url = task.Hostname;
            var res = ValidateCertificateByUrl(url);

            AbMonitorResult abMonitorResult = new AbMonitorResult
            {
                ResultId = new Guid(guid),
                ConfigId = new Guid(configID),
                TaskId = new Guid(task.Id),
                TimeStamp = DateTime.Now,
                TaskType = task.Type,
                Result = res ? "Verified" : "failed",
                Level = 1,
                CustomerId = new Guid(customerId)
            };
            if (!_dataCtr.CreateResult(abMonitorResult))
                _logger.LogError("Something went wrong while trying to save the data.");

            await Task.CompletedTask;
        }


        //Test Abilita-1802
        private bool ValidateCertificateByHostName(string hostname)
        {
            X509Store myX509Store = new X509Store(StoreName.My, StoreLocation.LocalMachine);

            myX509Store.Open(OpenFlags.ReadWrite);

            X509Certificate2 myCertificate = myX509Store.Certificates.OfType<X509Certificate2>()
                .FirstOrDefault(cert => cert.GetNameInfo(X509NameType.SimpleName, false) == hostname);

            if (myCertificate.Verify())
                return true;

            return false;
        }
        //if it is stored in a file
        private static bool ValidateCertificateByFileName(string filename)
        {
            X509Certificate2 Cert = new X509Certificate2(filename);
            if (Cert.Verify())
                return true;

            return false;

        }

        private bool ValidateCertificateByUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return false;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);//HttpWebRequest.CreateHttp(url);
            request.ServerCertificateValidationCallback += ServerCertificateValidationCallback;
            HttpWebResponse resp = (HttpWebResponse)request.GetResponse();
            resp.Close();
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse()) { return true;/* var cert = request.ServicePoint.Certificate;*/ /*not for core*/ }
            }
            catch (Exception ex)
            {
                _logger.LogError("", ex);
            }
            return false;
        }
        private bool ServerCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {

            if (sslPolicyErrors == SslPolicyErrors.None)
            {
                X509Certificate2 x509Certificate2 = new X509Certificate2(certificate);
                //Test verification

                return x509Certificate2.Verify();
            }
            else
                _logger.LogInformation($"Certificate Error: {sslPolicyErrors.ToString()}");

            _logger.LogError($"Validating certificate {certificate.Issuer} error: {sslPolicyErrors}");
            return false;
        }
    }
}
