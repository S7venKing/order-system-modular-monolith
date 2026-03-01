using Humanizer.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using order_system_modular_monolith.BuildingBlocks.EFCore;
using order_system_modular_monolith.Product.Data;

namespace order_system_modular_monolith.Product.Extensions.Infrastructure
{
    public static class InfrastructureExtension
    {
        public static WebApplicationBuilder AddProductModules(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            builder.Services.AddCustomMediatR();
            builder.Services.AddDbContext<ProductDbContext>(options =>
                     options.UseNpgsql(
                            configuration.GetConnectionString("ordersdb")));
            
            return builder;
        }

        public static WebApplication UseProductModules(this WebApplication app)
        {
            app.UseMigration<ProductDbContext>();
            return app;
        }
    }
}
