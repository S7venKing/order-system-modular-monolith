using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.EntityFrameworkCore;
using order_system_modular_monolith.BuildingBlocks.Application.Abstractions;
using order_system_modular_monolith.BuildingBlocks.Infrastructure;
using order_system_modular_monolith.BuildingBlocks.Jwt;
using order_system_modular_monolith.BuildingBlocks.Web;

namespace order_system_modular_monolith.Api.Extension
{
    public static class SharedInfrastructureExtensions
    {
        public static WebApplicationBuilder AddSharedInfrastructure(this WebApplicationBuilder builder)
        {
            builder.AddServiceDefaults();
            builder.Services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();
            builder.Services.AddScoped<IDateTimeProvider, DateTimeProvider>();
            builder.Services.AddJwt();
            return builder;
        }
    }
}
