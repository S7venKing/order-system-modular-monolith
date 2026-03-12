using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using order_system_modular_monolith.BuildingBlocks.Infrastructure;
using order_system_modular_monolith.Order.Order.Infrastructure.Redis;

namespace order_system_modular_monolith.Order.Order.Infrastructure
{
    public static class InfrastructureExtension
    {
        public static WebApplicationBuilder AddOrderModule(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            // Register RedisHelper from BuildingBlocks if Redis configured in shared infra
            builder.Services.AddSingleton<RedisHelper>(sp =>
            {
                var conn = sp.GetService<StackExchange.Redis.IConnectionMultiplexer>();
                if (conn == null) throw new InvalidOperationException("IConnectionMultiplexer not registered. Ensure Redis is configured via shared infra.");
                return new RedisHelper(conn);
            });

            builder.Services.AddTransient<CartRedisRepository>();

            return builder;
        }

        public static WebApplication UseOrderModule(this WebApplication app)
        {
            return app;
        }
    }
}
