using MediatR;
using Microsoft.Extensions.DependencyInjection;
using order_system_modular_monolith.Module.Stock;
using order_system_modular_monolith.Stock.Root;
using order_system_modular_monolith.Stock.Stock.Service;

namespace order_system_modular_monolith.Stock.Extensions.Infrastructure
{
    public static class MediatRExtensions
    {
        public static IServiceCollection AddCustomMediatR(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(StockRoot).Assembly));

            return services;
        }
    }
}
