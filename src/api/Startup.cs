using command.Handlers;
using command.persistence.Context;
using MediatR;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OData.Edm;
using query.Handlers;
using query.models;
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
            services.AddDbContext<ApplicationContext>(ctx =>
            {
                ctx.UseSqlServer(Configuration.GetConnectionString("cerberus-command"));
            });

            services.AddTransient<ApplicationQueryContext>();

            services.AddMediatR(
                typeof(CreateNewCustomerHandler).Assembly,
                typeof(GetAllCustomersHandler).Assembly);

            services.AddMvc();

            services.AddOData();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseMvc(routeBuilder =>
            {
                routeBuilder.Expand().Select().Filter().Count().OrderBy();

                routeBuilder.MapODataServiceRoute("customers", "bob", EdmModelBuilder.GetCustomersModel());
            });
        }
    }

    public class EdmModelBuilder
    {
        public static IEdmModel GetCustomersModel()
        {
            var builder = new ODataConventionModelBuilder();

            builder.EntitySet<Customer>("customers");

            return builder.GetEdmModel();
        }
    }
}
