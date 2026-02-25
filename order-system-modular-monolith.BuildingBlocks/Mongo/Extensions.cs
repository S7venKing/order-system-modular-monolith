using Humanizer.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace order_system_modular_monolith.BuildingBlocks.Mongo
{
    public static class MongoExtensions
    {
        public static IServiceCollection AddMongoDbContext<TContext>(
            this IServiceCollection services,
            IConfiguration configuration,
            Action<MongoOptions>? configurator = null)
            where TContext : MongoDbContext
        {
            return services.AddMongoDbContext<TContext, TContext>(
                configuration,
                configurator);
        }

        public static IServiceCollection AddMongoDbContext<TContextService, TContextImplementation>(
            this IServiceCollection services,
            IConfiguration configuration,
            Action<MongoOptions>? configurator = null)
            where TContextService : class, IMongoDbContext
            where TContextImplementation : MongoDbContext, TContextService
        {
            // Bind options from config
            services.AddMongoDbContext<MongoDbContext>(configuration, opts => configuration.GetSection("Mongo"));

            services.PostConfigure<MongoOptions>(options =>
            {
                var aspireConnectionString = configuration.GetConnectionString("mongo");
                if (!string.IsNullOrWhiteSpace(aspireConnectionString))
                {
                    options.ConnectionString = aspireConnectionString;
                }
            });

            // Override with Aspire connection string if exists
            services.PostConfigure<MongoOptions>(options =>
            {
                var aspireConnectionString = configuration.GetConnectionString("mongo");
                if (!string.IsNullOrWhiteSpace(aspireConnectionString))
                {
                    options.ConnectionString = aspireConnectionString;
                }
            });

            // Apply custom configurator
            if (configurator is not null)
            {
                services.Configure(configurator);
            }

            // Register MongoClient as Singleton
            services.AddSingleton<IMongoClient>(sp =>
            {
                var mongoOptions = sp.GetRequiredService<IOptions<MongoOptions>>().Value;
                return new MongoClient(mongoOptions.ConnectionString);
            });

            // Register context
            services.AddScoped<TContextService, TContextImplementation>();
            services.AddScoped<TContextImplementation>();
            services.AddScoped<IMongoDbContext>(sp =>
                sp.GetRequiredService<TContextService>());

            // Repository & UnitOfWork
            services.AddScoped(typeof(IMongoRepository<,>), typeof(MongoRepository<,>));
            services.AddScoped(typeof(IMongoUnitOfWork<>), typeof(MongoUnitOfWork<>));

            return services;
        }
    }
}