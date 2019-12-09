using System.Threading.Tasks;
using consumer.persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace order.consumer
{
    public delegate MongoStorage GetStorage();

    public class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging((context, builder) => builder.AddConsole())
                .ConfigureAppConfiguration(context => { context.AddJsonFile("appsettings.json"); })
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<GetStorage>(() => new MongoStorage(context.Configuration));

                    services.AddHostedService<Worker>();
                });
    }
}
