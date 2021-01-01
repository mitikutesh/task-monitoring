using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

namespace Monitoring.Service.Services
{
    public class MonitorBase : ServiceBase, IHostLifetime
    {
        private readonly TaskCompletionSource<object> _delayStart;
        private IHostApplicationLifetime ApplicationLifetime { get; }

        public MonitorBase(IHostApplicationLifetime applicationLifetime)
        {
            _delayStart = new TaskCompletionSource<object>();
            ApplicationLifetime = applicationLifetime ?? throw new
            ArgumentNullException(nameof(applicationLifetime));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Stop();
            return Task.CompletedTask;
        }

        public Task WaitForStartAsync(CancellationToken cancellationToken)
        {
            cancellationToken.Register(() => _delayStart.TrySetCanceled());
            ApplicationLifetime.ApplicationStopping.Register(Stop);
            // Otherwise this would block and prevent IHost.StartAsync from finishing.
            new Thread(Run).Start();
            return _delayStart.Task;
        }

        private void Run()
        {
            try
            {
                Run(this); // This blocks until the service is stopped.
                _delayStart.TrySetException(new
                 InvalidOperationException("Stopped without starting"));
            }
            catch (Exception ex)
            {
                File.AppendAllText(@"C:\temp\MonitorService\Service.Exception.txt", DateTime.Now.ToString() + ex.Message);
                _delayStart.TrySetException(ex);
            }
        }

        protected override void OnStart(string[] args)
        {
            _delayStart.TrySetResult(null);
            base.OnStart(args);

        }
        protected override void OnStop()
        {
            ApplicationLifetime.StopApplication();
            base.OnStop();
        }

        protected override void OnPause()
        {
            // Custom action on pause
            base.OnPause();
        }

        protected override void OnContinue()
        {
            // Custom action on continue
            base.OnContinue();
        }
    }
}
