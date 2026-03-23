using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using order_system_modular_monolith.Identity.Data;
using order_system_modular_monolith.Identity.Models;
using order_system_modular_monolith.Module.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.DataProtection;

namespace order_system_modular_monolith.Identity.Extensions.Infrastructure;

public static class IdentityServerExtensions
{
    public static WebApplicationBuilder AddCustomIdentityServer(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddDataProtection()
            .PersistKeysToFileSystem(new DirectoryInfo("/app/dpkeys"))
            .SetApplicationName("orders-api");

        builder.Services.AddIdentity<User, Role>(config =>
        {
            config.Password.RequiredLength = 6;
            config.Password.RequireDigit = false;
            config.Password.RequireNonAlphanumeric = false;
            config.Password.RequireUppercase = false;
        })
        .AddEntityFrameworkStores<IdentityContext>()
        .AddDefaultTokenProviders();

        var identityServerBuilder = builder.Services
            .AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
            })
            .AddInMemoryIdentityResources(Config.IdentityResources)
            .AddInMemoryApiResources(Config.ApiResources)
            .AddInMemoryApiScopes(Config.ApiScopes)
            .AddInMemoryClients(Config.Clients)
            .AddAspNetIdentity<User>()
            .AddResourceOwnerValidator<UserValidator>()
            .AddKeyManagement();

        //identityServerBuilder.AddDeveloperSigningCredential();

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.Events.OnRedirectToLogin = context =>
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            };

            options.Events.OnRedirectToAccessDenied = context =>
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return Task.CompletedTask;
            };
        });

        return builder;
    }
}
