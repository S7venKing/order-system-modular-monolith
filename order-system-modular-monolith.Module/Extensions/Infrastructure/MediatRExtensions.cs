using MediatR;
using Microsoft.Extensions.DependencyInjection;
using order_system_modular_monolith.Module.Product;
using order_system_modular_monolith.Product.Data;
using order_system_modular_monolith.Product.Product.Domain;
using order_system_modular_monolith.Product.Root;

namespace order_system_modular_monolith.Product.Extensions.Infrastructure
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
