using Humanizer.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using order_system_modular_monolith.BuildingBlocks.Application.Abstractions;
using order_system_modular_monolith.BuildingBlocks.EFCore;
using order_system_modular_monolith.BuildingBlocks.Web;
using order_system_modular_monolith.Stock.Data;
using order_system_modular_monolith.Stock.Stock.Repository;
using order_system_modular_monolith.Stock.Stock.Service;
using order_system_modular_monolith.Stock.Repository;

namespace order_system_modular_monolith.Stock.Extensions.Infrastructure
{
    public static class InfrastructureExtension
    {
        public static WebApplicationBuilder AddStockModules(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            builder.Services.AddCustomMediatR();
            builder.Services.AddDbContext<StockDbContext>(options =>
                     options.UseNpgsql(
                            configuration.GetConnectionString("Postgres")));
            builder.Services.AddTransient<IStockRepository, StockRepository>();
            builder.Services.AddTransient<CreateStockHandler>();
            builder.Services.AddTransient<UpdateStockHandler>();
            return builder;
        }

        public static WebApplication UseStockModules(this WebApplication app)
        {
            app.UseMigration<StockDbContext>();

            return app;
        }
    }
}
