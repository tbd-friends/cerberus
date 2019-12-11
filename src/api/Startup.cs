using command.Handlers;
using command.persistence.Context;
using command.persistence.Models;
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
using query.persistence;
using Customer = query.models.Customer;

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

            services.AddMvc(options => options.EnableEndpointRouting = false);

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

                routeBuilder.MapODataServiceRoute("addresses", "odata", EdmModelBuilder.GetAddressesModel());
                routeBuilder.MapODataServiceRoute("customers", "odata", EdmModelBuilder.GetCustomersModel());
            });
        }
    }

    public class EdmModelBuilder
    {
        public static IEdmModel GetAddressesModel()
        {
            var builder = new ODataConventionModelBuilder();

            builder.EntityType<CustomerAddress>().HasKey(k => new { k.CustomerId, k.AddressId });

            builder.EntitySet<CustomerAddress>("Addresses");

            return builder.GetEdmModel();
        }

        public static IEdmModel GetCustomersModel()
        {
            var builder = new ODataConventionModelBuilder();

            builder.EntitySet<Customer>("Customers");

            return builder.GetEdmModel();
        }
    }
}
