using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace NestedHosting.NestedHosting
{
    public class NestedHostLifetime : IHostLifetime
    {
        private CancellationTokenRegistration _applicationStartedRegistration;
        private CancellationTokenRegistration _applicationStoppingRegistration;
        
        private ILogger Logger { get; }
        private IHostApplicationLifetime ApplicationLifetime { get; }

        public NestedHostLifetime(IHostApplicationLifetime applicationLifetime, ILogger<NestedHostLifetime> logger)
        {
            ApplicationLifetime = applicationLifetime;
            Logger = logger;
        }
        
        public Task WaitForStartAsync(CancellationToken cancellationToken)
        {
            _applicationStartedRegistration = ApplicationLifetime.ApplicationStarted.Register(state =>
                {
                    ((NestedHostLifetime)state).OnApplicationStarted();
                },
                this);
            _applicationStoppingRegistration = ApplicationLifetime.ApplicationStopping.Register(state =>
                {
                    ((NestedHostLifetime)state).OnApplicationStopping();
                },
                this);
            
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _applicationStartedRegistration.Dispose();
            _applicationStoppingRegistration.Dispose();
            
            return Task.CompletedTask;
        }
        
        private void OnApplicationStarted()
        {
            Logger.LogInformation("Nested application started");
        }

        private void OnApplicationStopping()
        {
            Logger.LogInformation("Nested application shutting down...");
        }
    }
}