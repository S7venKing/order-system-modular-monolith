using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace order_system_modular_monolith.BuildingBlocks.Infrastructure
{
    public static class RedisExtensions
    {
        public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
        {
            var cfg = configuration.GetValue<string>("RedisSettings:Connection")
                      ?? configuration.GetConnectionString("Redis")
                      ?? "localhost:6379";

            services.AddSingleton<IConnectionMultiplexer>(sp => ConnectionMultiplexer.Connect(cfg));

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = cfg;
            });

            return services;
        }
    }
}
