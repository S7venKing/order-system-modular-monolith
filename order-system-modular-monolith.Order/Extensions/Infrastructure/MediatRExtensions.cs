using MediatR;
using Microsoft.Extensions.DependencyInjection;
using order_system_modular_monolith.Order;

namespace order_system_modular_monolith.Order.Extensions.Infrastructure
{
    public static class MediatRExtensions
    {
        public static IServiceCollection AddCustomMediatR(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(OrderRoot).Assembly));

            return services;
        }
    }
}
