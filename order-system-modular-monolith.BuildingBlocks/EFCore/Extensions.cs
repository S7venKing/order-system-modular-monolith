using System.Linq.Expressions;
using Humanizer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using order_system_modular_monolith.BuildingBlocks.EFCore;
using order_system_modular_monolith.BuildingBlocks.Web;

namespace order_system_modular_monolith.BuildingBlocks.EFCore;

public static class Extensions
{
    public static IServiceCollection AddCustomDbContext<TContext>(
        this WebApplicationBuilder builder,
        string? connectionName = ""
    )
        where TContext : DbContext, IDbContext
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        builder.Services.AddValidateOptions<PostgresOptions>();

        builder.Services.AddDbContext<TContext>(
            (sp, options) =>
            {
                var aspireConnectionString = builder.Configuration.GetConnectionString(connectionName.Kebaberize());

                var connectionString =
                    aspireConnectionString
                    ?? builder.Configuration.GetSection($"PostgresOptions:ConnectionString:{connectionName}").Value;

                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new ArgumentException($"Connection string '{connectionName}' not found.");
                }

                options
                    .UseNpgsql(
                        connectionString,
                        dbOptions =>
                        {
                            dbOptions.MigrationsAssembly(typeof(TContext).Assembly.GetName().Name);
                        }
                    );

                options.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
            }
        );

        builder.Services.AddScoped<ISeedManager, SeedManager>();
        builder.Services.AddScoped<IDbContext>(sp => sp.GetRequiredService<TContext>());

        return builder.Services;
    }

    public static IApplicationBuilder UseMigration<TContext>(this IApplicationBuilder app)
        where TContext : DbContext, IDbContext
    {
        MigrateAsync<TContext>(app.ApplicationServices).GetAwaiter().GetResult();

        SeedAsync(app.ApplicationServices).GetAwaiter().GetResult();

        return app;
    }


    private static async Task MigrateAsync<TContext>(IServiceProvider serviceProvider)
        where TContext : DbContext, IDbContext
        {
        await using var scope = serviceProvider.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<TContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<TContext>>();

        await context.Database.MigrateAsync();

        //var pendingMigrations = await context.Database.GetPendingMigrationsAsync();

        //if (pendingMigrations.Any())
        //{
        //    logger.LogInformation("Applying {Count} pending migrations...", pendingMigrations.Count());

        //    await context.Database.MigrateAsync();
        //    logger.LogInformation("Migrations applied successfully.");
        //}
    }

    private static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        await using var scope = serviceProvider.CreateAsyncScope();

        var seedersManager = scope.ServiceProvider.GetRequiredService<ISeedManager>();

        await seedersManager.ExecuteSeedAsync();
    }
}