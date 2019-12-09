using System;
using System.Threading.Tasks;
using consumer.persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace customer.consumer
{
    public delegate MongoStorage GetStorage();

    class Program
    {
        static async Task Main(string[] args)
        {
            await GetBuilder(args).Build().RunAsync();
        }

        private static IHostBuilder GetBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureLogging((context, builder) => builder.AddConsole())
                .ConfigureAppConfiguration(context => { context.AddJsonFile("appsettings.json"); })
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<GetStorage>(() => new MongoStorage(context.Configuration));

                    services.AddHostedService<Worker>();
                });
        }
    }
}
