using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace NestedHosting
{
    public class WorkerService : BackgroundService
    {
        private readonly ILogger<WorkerService> _logger;

        public WorkerService(ILogger<WorkerService> logger)
        {
            _logger = logger;
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker service in nested code has started");

            var source = new TaskCompletionSource<int>();
            stoppingToken.Register(() => source.SetResult(0));

            await source.Task;
            
            _logger.LogInformation("Worker service is stopping...");

            await Task.Delay(TimeSpan.FromSeconds(2));
            
            _logger.LogInformation("Worker service is stopped");
        }
    }
}