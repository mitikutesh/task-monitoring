using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Monitoring.Service.Services
{
    public abstract class MonitoringBackgroundService : IHostedService
    {
        private readonly ILogger _logger;
        private readonly TaskCompletionSource<object> _delayStart;
        private System.Threading.Tasks.Task _executingTask;
        private readonly CancellationTokenSource _stoppingCts =
            new CancellationTokenSource();
        public MonitoringBackgroundService(
            ILogger<MonitoringBackgroundService> logger,
            IHostApplicationLifetime appLifetime)
        {
            _logger = logger;
            var hostApplicationLifetime = appLifetime ?? throw new ArgumentNullException(nameof(appLifetime));
            hostApplicationLifetime.ApplicationStarted.Register(OnStarted);
            hostApplicationLifetime.ApplicationStopping.Register(OnStopping);
            hostApplicationLifetime.ApplicationStopped.Register(OnStopped);
        }

        protected virtual async System.Threading.Tasks.Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(() =>
                _logger.LogDebug($" GracePeriod background task is stopping."));
            do
            {
                await ProcessAsync();
            }
            while (!stoppingToken.IsCancellationRequested);
        }

        protected abstract System.Threading.Tasks.Task ProcessAsync();
        public virtual System.Threading.Tasks.Task StartAsync(CancellationToken cancellationToken)
        {
            var text = $"{DateTime.Now.ToString("yyyy-MM-dd HH: mm: ss")}, Monitor Service started." + Environment.NewLine;
            File.AppendAllText(@"C:\temp\MonitorService\Service.Write.txt", text);
            _logger.LogInformation("Monitor service started.");

            _executingTask = ExecuteAsync(_stoppingCts.Token);
            if (_executingTask.IsCompleted)
            {
                return _executingTask;
            }

            // Otherwise it's running
            return Task.CompletedTask;
        }
        public virtual async System.Threading.Tasks.Task StopAsync(CancellationToken cancellationToken)
        {
            var text = $"{DateTime.Now.ToString("yyyy-MM-dd HH: mm: ss")}, Monitor service stopped." + Environment.NewLine;
            File.AppendAllText(@"D:\temp\MonitoringService\Service.Write.txt", text);

            // Stop called without start
            if (_executingTask == null)
            {
                return;
            }
            try
            {
                // Signal cancellation to the executing method
                _stoppingCts.Cancel();
            }
            finally
            {
                // Wait until the task completes or the stop token triggers
                await Task.WhenAny(_executingTask, Task.Delay(Timeout.Infinite,
                    cancellationToken));
            }
        }

        private void OnStarted()
        {
            _logger.LogInformation("2. OnStarted has been called.");
        }

        private void OnStopping()
        {
            _logger.LogInformation("3. OnStopping has been called.");
        }

        private void OnStopped()
        {
            _logger.LogInformation("5. OnStopped has been called.");
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}