using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using query.persistence;

namespace order.consumer
{
    public delegate ApplicationQueryContext GetContext();

    public class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(context => { context.AddJsonFile("appsettings.json"); })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<GetContext>(() => new ApplicationQueryContext());

                    services.AddHostedService<Worker>();
                });
    }
}
