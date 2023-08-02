using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace NestedHosting.NestedHosting
{
    public abstract class NestedHostContainerService : BackgroundService
    {
        protected readonly ILogger Logger;

        protected NestedHostContainerService(ILogger logger)
        {
            Logger = logger;
        }
        
        protected abstract Task PreStartAsync(CancellationToken stoppingToken);
        
        protected abstract Task RunNestedHostAsync(CancellationToken stoppingToken);
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Logger.LogInformation("Executing nested host container service pre-start handler");
            await PreStartAsync(stoppingToken);
            Logger.LogInformation("Pre-start step complete. Starting nested application...");
            await RunNestedHostAsync(stoppingToken);
            Logger.LogInformation("Nested host container service was stopped");
        }
    }
}