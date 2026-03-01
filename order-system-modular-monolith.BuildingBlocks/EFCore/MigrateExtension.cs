using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace order_system_modular_monolith.BuildingBlocks.EFCore
{
    public static class MigrationExtensions
    {

        public static void MigrateDbContext<TContext>(this IServiceProvider serviceProvider)
           where TContext : DbContext
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TContext>();

            var retry = 5;

            while (retry > 0)
            {
                try
                {
                    if (context.Database.GetPendingMigrations().Any())
                    {
                        context.Database.Migrate();
                    }

                    break;
                }
                catch (Exception)
                {
                    retry--;
                    Thread.Sleep(2000);

                    if (retry == 0)
                        throw;
                }
            }
        }

        public static void MigrateDbContexts(this IServiceProvider serviceProvider, params Type[] dbContextTypes)
        {
            using var scope = serviceProvider.CreateScope();

            foreach (var type in dbContextTypes)
            {
                if (!typeof(DbContext).IsAssignableFrom(type))
                    continue;

                var context = (DbContext)scope.ServiceProvider.GetRequiredService(type);
                context.Database.Migrate();
            }
        }
    }
}