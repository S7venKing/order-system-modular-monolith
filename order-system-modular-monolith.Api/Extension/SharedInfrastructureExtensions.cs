using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.EntityFrameworkCore;
using order_system_modular_monolith.BuildingBlocks.Jwt;

namespace order_system_modular_monolith.Api.Extension
{
    public static class SharedInfrastructureExtensions
    {
        public static WebApplicationBuilder AddSharedInfrastructure(this WebApplicationBuilder builder)
        {
            builder.AddServiceDefaults();
            builder.Services.AddJwt();

 
            return builder;
        }
    }
}
