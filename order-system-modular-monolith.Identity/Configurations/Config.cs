using Duende.IdentityModel;
using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using order_system_modular_monolith.Identity.Constants;

namespace order_system_modular_monolith.Module.Configurations;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email()
        };


    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            // Define API scope for the Order System and request role claims in access token
            new ApiScope(Constants.StandardScopes.OrderSystem, new List<string> { JwtClaimTypes.Role })
        };


    public static IList<ApiResource> ApiResources =>
        new List<ApiResource>
        {
new(Constants.StandardScopes.OrderSystem)
{
    Scopes = { Constants.StandardScopes.OrderSystem },
    UserClaims = { JwtClaimTypes.Role }
}
        };

    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
new Client
{
    ClientId = "client",

    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

    ClientSecrets =
    {
        new Secret("secret".Sha256())
    },

    AllowedScopes =
    {
        IdentityServerConstants.StandardScopes.OpenId,
        IdentityServerConstants.StandardScopes.Profile,
        Constants.StandardScopes.OrderSystem
    },

    AccessTokenLifetime = 3600,
    IdentityTokenLifetime = 3600,
}
        ,
        // Swagger UI / Scalar interactive client
        new Client
        {
            ClientId = "swagger-ui",
            ClientName = "Swagger UI",
            AllowedGrantTypes = GrantTypes.Code,
            RequirePkce = true,
            RequireClientSecret = false,
            RedirectUris = { "https://localhost:5003/oauth2-redirect.html" }, // Swagger UI redirect
            PostLogoutRedirectUris = { "https://localhost:5003/swagger" },
            AllowedCorsOrigins = { "https://localhost:5003" },
            AllowedScopes =
            {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                Constants.StandardScopes.OrderSystem,
            }
        }
        };
}