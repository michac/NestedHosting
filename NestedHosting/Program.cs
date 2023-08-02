using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NestedHosting.NestedHosting;

namespace NestedHosting
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder();

            builder.Services.AddWindowsService();

            builder.Services.AddHostedService<ApplicationService>();

            var host = builder.Build();

            host.Run();
        }
    }
}