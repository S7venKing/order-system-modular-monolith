using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace order_system_modular_monolith.Identity.Extensions.Infrastructure;

using Configurations;

public static class MediatRExtensions
{
    public static IServiceCollection AddCustomMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(IdentityRoot).Assembly));


        return services;
    }
}