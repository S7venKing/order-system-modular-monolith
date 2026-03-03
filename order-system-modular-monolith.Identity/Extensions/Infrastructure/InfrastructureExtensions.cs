using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using order_system_modular_monolith.BuildingBlocks.EFCore;
using order_system_modular_monolith.Identity.Data;
using order_system_modular_monolith.Identity.Data.Seed;

namespace order_system_modular_monolith.Identity.Extensions.Infrastructure;


public static class InfrastructureExtensions
{
    public static WebApplicationBuilder AddIdentityModules(this WebApplicationBuilder builder, IConfiguration configuration)
    {
        builder.AddCustomDbContext<IdentityContext>("Postgres");
        builder.Services.AddScoped<IDataSeeder, IdentityDataSeeder>();
        builder.AddCustomIdentityServer();


        builder.Services.AddCustomMediatR();

        return builder;
    }


    public static WebApplication UseIdentityModules(this WebApplication app)
    {
        app.UseForwardedHeaders();
        app.UseIdentityServer();
        app.UseMigration<IdentityContext>();

        return app;
    }
}