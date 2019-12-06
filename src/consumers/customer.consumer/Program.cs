using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using query.persistence;

namespace customer.consumer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await GetBuilder(args).Build().RunAsync();
        }

        private static IHostBuilder GetBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureAppConfiguration(context => { context.AddJsonFile("appsettings.json"); })
                .ConfigureServices(services =>
                {
                    services.AddSingleton<GetContext>(() => new ApplicationQueryContext());

                    services.AddHostedService<WorkerService>();
                });
        }
    }
}
