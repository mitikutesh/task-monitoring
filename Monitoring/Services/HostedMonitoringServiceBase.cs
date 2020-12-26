using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Monitoring.Services
{
    public class HostedMonitoringServiceBase : IHostedService
    {
        private readonly ILogger _logger;
        private readonly TaskCompletionSource<object> _delayStart;
        public HostedMonitoringServiceBase(
            ILogger<HostedMonitoringServiceBase> logger,
            IHostApplicationLifetime appLifetime)
        {
            _logger = logger;
            
            var hostApplicationLifetime = appLifetime ?? throw new ArgumentNullException(nameof(appLifetime));
            hostApplicationLifetime.ApplicationStarted.Register(OnStarted);
            hostApplicationLifetime.ApplicationStopping.Register(OnStopping);
            hostApplicationLifetime.ApplicationStopped.Register(OnStopped);
        }


        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("1. StartAsync has been called.");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("4. StopAsync has been called.");

            return Task.CompletedTask;
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
    }
}