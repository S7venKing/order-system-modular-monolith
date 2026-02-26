using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.EntityFrameworkCore;

namespace order_system_modular_monolith.Api.Extension
{
    public static class SharedInfrastructureExtensions
    {
        public static WebApplicationBuilder AddSharedInfrastructure(this WebApplicationBuilder builder)
        {
            builder.AddServiceDefaults();


            return builder;
        }
    }
}
