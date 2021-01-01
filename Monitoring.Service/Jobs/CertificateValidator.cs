using Microsoft.Extensions.Logging;
using Monitoring.Data.Entities;
using Monitoring.Data.Interfaces;
using Monitoring.Infrastructure.Models;
using Monitoring.Service.Interfaces;
using Monitoring.Service.Services;
using System;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Monitoring.Service.Jobs
{
    public class CertValidator : ScheduledProcessor, ICertValidator
    {
        ILogger<CertValidator> _logger;

        public CertValidator(IDataController dataCtr, ILogger<CertValidator> logger) : base(dataCtr, logger)
        {
            _logger = logger;
        }


        public override async Task<bool> IsInitDataOk(TasksToDo task, string configID, string customerID)
        {
            bool IsValid(string config)
            {
                return Guid.TryParse(config, out Guid guid);
            }

            if (string.IsNullOrWhiteSpace(task.HostName) || string.IsNullOrWhiteSpace(configID) || string.IsNullOrWhiteSpace(customerID) || !task.HostName.StartsWith("https://"))
            {
                return await Task.FromResult(false);
            }
            if (IsValid(configID) && IsValid(customerID))
                return await Task.FromResult(true);

            return await Task.FromResult(false);
        }

        public override async Task Process(TasksToDo task, string configID, string customerId, string guid)
        {
            if (!await IsDoTaskOk(task, configID, customerId))
                return;

            var url = task.HostName;
            var res = ValidateCertificateByUrl(url);

            MonitoringReport abMonitorResult = new MonitoringReport
            {
                Id = new Guid(guid),
                ConfigId = new Guid(configID),
                TaskId = task.Id,
                TimeStamp = DateTime.Now,
                TaskType = task.Type,
                Result = res ? "Verified" : "failed",
                Level = 1,
                ClientId = new Guid(customerId)
            };
            //if (!_dataCtr.CreateResult(abMonitorResult))
            //    _logger.LogError("Something went wrong while trying to save the data.");

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
