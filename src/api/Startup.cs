using api.GraphQL;
using command.Handlers;
using command.persistence.Context;
using GraphQL.Server;
using GraphQL.Server.Transports.AspNetCore;
using GraphQL.Types;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using query.Handlers;
using query.persistence;

namespace api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<KestrelServerOptions>(options => { options.AllowSynchronousIO = true; });
            services.Configure<IISServerOptions>(options => { options.AllowSynchronousIO = true; });

            services.AddControllers();

            services.AddDbContext<ApplicationContext>(ctx =>
            {
                ctx.UseSqlServer(Configuration.GetConnectionString("cerberus-command"));
            });

            services.AddTransient<ApplicationQueryContext>();

            services.AddMediatR(
                typeof(CreateNewCustomerHandler).Assembly,
                typeof(GetAllCustomersHandler).Assembly);

            services.AddTransient<CustomerType>();
            services.AddTransient<AddressType>();
            services.AddTransient<CustomerOrderType>();
            services.AddTransient<CerberusQuery>();
            services.AddSingleton<ISchema, CerberusSchema>();
            services.AddHttpContextAccessor();

            services.AddGraphQL(options =>
            {
                options.EnableMetrics = true;
                options.ExposeExceptions = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseGraphQL<ISchema>();

            app.UseGraphQLPlayground();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
