using Humanizer.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using order_system_modular_monolith.BuildingBlocks.Application.Abstractions;
using order_system_modular_monolith.BuildingBlocks.EFCore;
using order_system_modular_monolith.BuildingBlocks.Web;
using order_system_modular_monolith.Product.Data;
using order_system_modular_monolith.Product.Product.Repository;
using order_system_modular_monolith.Product.Product.Service;
using order_system_modular_monolith.Product.Repository;

namespace order_system_modular_monolith.Product.Extensions.Infrastructure
{
    public static class InfrastructureExtension
    {
        public static WebApplicationBuilder AddProductModules(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            builder.Services.AddCustomMediatR();
            builder.Services.AddDbContext<ProductDbContext>(options =>
                     options.UseNpgsql(
                            configuration.GetConnectionString("Postgres")));
            builder.Services.AddTransient<IProductRepository, ProductRepository>();
            builder.Services.AddTransient<CreateProductHandler>();
            return builder;
        }

        public static WebApplication UseProductModules(this WebApplication app)
        {
            app.UseMigration<ProductDbContext>();

            return app;
        }
    }
}
