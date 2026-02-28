using MediatR;
using Microsoft.Extensions.DependencyInjection;
using order_system_modular_monolith.Module.Product;

namespace order_system_modular_monolith.Module.Product.Infrastructure
{
    public static class MediatRExtensions
    {
        public static IServiceCollection AddCustomMediatR(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(ProductRoot).Assembly));

            return services;
        }
    }
}
