using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NestedHosting.NestedHosting;

namespace NestedHosting
{
    public class ApplicationService : NestedHostContainerService
    {
        public ApplicationService(ILogger<ApplicationService> logger) : base(logger)
        {
        }

        protected override async Task PreStartAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }

        protected override async Task RunNestedHostAsync(CancellationToken stoppingToken)
        {
            var builder = Host.CreateApplicationBuilder();
             
             builder.Services.AddSingleton<IHostLifetime, NestedHostLifetime>();
             builder.Services.AddHostedService<WorkerService>();
            
             var host = builder.Build();
             
             await host.RunAsync(stoppingToken);
        }
    }
}