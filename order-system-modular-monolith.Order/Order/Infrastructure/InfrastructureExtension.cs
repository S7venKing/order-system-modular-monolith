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
            // register MediatR for Order module
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(InfrastructureExtension).Assembly));

            // Register RedisHelper from BuildingBlocks if Redis configured in shared infra
            builder.Services.AddSingleton<RedisHelper>(sp =>
            {
                var conn = sp.GetService<StackExchange.Redis.IConnectionMultiplexer>();
                if (conn == null) throw new InvalidOperationException("IConnectionMultiplexer not registered. Ensure Redis is configured via shared infra.");
                return new RedisHelper(conn);
            });

            builder.Services.AddTransient<CartRedisRepository>();

            // register MediatR handlers and http context accessor
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddTransient<order_system_modular_monolith.Order.Order.Handlers.AddToCartHandler>();
            builder.Services.AddTransient<order_system_modular_monolith.Order.Order.Handlers.GetCartHandler>();
            builder.Services.AddTransient<order_system_modular_monolith.Order.Order.Handlers.RemoveFromCartHandler>();

            // configure gRPC clients for Product and Stock services
            var productAddr = configuration["Services:Product"] ?? "https://localhost:6001";
            var stockAddr = configuration["Services:Stock"] ?? "https://localhost:6002";

            builder.Services.AddGrpcClient<order_system_modular_monolith.Product.Protos.ProductService.ProductServiceClient>(opt =>
            {
                opt.Address = new Uri(productAddr);
            });

            builder.Services.AddGrpcClient<order_system_modular_monolith.Stock.Protos.StockService.StockServiceClient>(opt =>
            {
                opt.Address = new Uri(stockAddr);
            });

            return builder;
        }

        public static WebApplication UseOrderModule(this WebApplication app)
        {
            return app;
        }
    }
}
