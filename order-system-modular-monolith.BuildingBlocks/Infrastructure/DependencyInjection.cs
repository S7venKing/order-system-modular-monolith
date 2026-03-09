using Microsoft.Extensions.DependencyInjection;
using order_system_modular_monolith.BuildingBlocks.Application.Abstractions;
using order_system_modular_monolith.BuildingBlocks.Domain;
using order_system_modular_monolith.BuildingBlocks.Web;
using System;
using System.Collections.Generic;
using System.Text;

namespace order_system_modular_monolith.BuildingBlocks.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBuildingBlocksInfrastructure(
            this IServiceCollection services)
        {
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();
            // Redis is optional; modules should call AddRedis when needed via WebApplicationBuilder extensions
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(DomainEventDispatcher).Assembly);
                cfg.RegisterServicesFromAssemblies(typeof(DomainEvent).Assembly
);

            });

            return services;
        }
    }
}
