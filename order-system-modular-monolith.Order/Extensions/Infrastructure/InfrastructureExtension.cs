using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using order_system_modular_monolith.BuildingBlocks.Application.Abstractions;
using order_system_modular_monolith.BuildingBlocks.EFCore;
using order_system_modular_monolith.BuildingBlocks.Web;
using order_system_modular_monolith.Order.Data;
using order_system_modular_monolith.Order.Repository;
using order_system_modular_monolith.Order.Order.Service;

namespace order_system_modular_monolith.Order.Extensions.Infrastructure
{
    public static class InfrastructureExtension
    {
        public static WebApplicationBuilder AddOrderModules(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            // register mediatr and dbcontext similar to Product/Stock modules
            builder.Services.AddCustomMediatR();
            builder.Services.AddDbContext<OrderDbContext>(options =>
                     options.UseNpgsql(
                            configuration.GetConnectionString("Postgres")));

            // register repository and handlers like other modules
            builder.Services.AddTransient<IOrderRepository, OrderRepository>();
            builder.Services.AddTransient<CreateOrderHandler>();

            return builder;
        }

        public static WebApplication UseOrderModules(this WebApplication app)
        {
            app.UseMigration<OrderDbContext>();
            return app;
        }
    }
}
