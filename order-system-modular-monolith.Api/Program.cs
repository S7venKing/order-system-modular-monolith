using Microsoft.OpenApi;
using order_system_modular_monolith.Api.Extension;
using order_system_modular_monolith.BuildingBlocks.Infrastructure;
using order_system_modular_monolith.Identity.Extensions.Infrastructure;
using order_system_modular_monolith.Product.Extensions.Infrastructure;
using order_system_modular_monolith.Stock.Extensions.Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddBuildingBlocksInfrastructure();
builder.Services.AddEndpointsApiExplorer();

var authority = builder.Configuration["Jwt:Authority"] ?? "https://localhost:5001";
var audience = builder.Configuration["Jwt:Audience"] ?? "order-system-modular-monolith";
var authorityUri = new Uri(authority.TrimEnd('/'));

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Order System API", Version = "v1" });

    // Định nghĩa OAuth2 scheme đúng cách (Type = OAuth2, có Flows)
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri(authorityUri, "connect/authorize"),
                TokenUrl = new Uri(authorityUri, "connect/token"),
                Scopes = new Dictionary<string, string>
                {
                    { "openid", "OpenID Connect" },
                    { "profile", "User profile" },
                    { audience, "Full access to Order System API" },
                    { $"{audience}.read", "Read access" },
                    { $"{audience}.write", "Write access" }
                }
            }
        },
        Description = "OAuth2 / OpenID Connect với PKCE (login popup để lấy JWT token)"
    });

    // Apply requirement toàn cục - dùng lambda + document! để fix nullable warning
    options.AddSecurityRequirement((document) => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference("oauth2", document!),
            new List<string> { "openid", "profile", audience }
        }
    });
});

builder.AddProductModules(builder.Configuration);
builder.AddIdentityModules(builder.Configuration);
builder.AddStockModules(builder.Configuration);
builder.AddSharedInfrastructure();

var app = builder.Build();

app.UseProductModules();
app.UseIdentityModules();

app.UseHttpsRedirection();

// Authentication & Authorization pipeline
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseHttpsRedirection();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("Order System API - Scalar UI")
            .WithTheme(ScalarTheme.DeepSpace)
            .WithOpenApiRoutePattern("/swagger/v1/swagger.json")
            // Setup OAuth2 Authorization Code + PKCE
            .AddPreferredSecuritySchemes("oauth2")
            .AddAuthorizationCodeFlow("oauth2", flow =>
            {
                flow.ClientId = "swagger-ui";  // ClientId public (đăng ký ở IdentityServer)
                // KHÔNG cần ClientSecret vì dùng PKCE
                flow.Pkce = Pkce.Sha256;  // Bật PKCE (Scalar hỗ trợ tự động)
                flow.SelectedScopes = new[]
                {
                    "openid",
                    "profile",
                    audience,
                    $"{audience}.read",
                    $"{audience}.write"
                };
                // Optional: Fix bug redirect_uri ở một số version Scalar (thêm nếu gặp 404 trước đó)
                flow.RedirectUri = "https://localhost:58551/scalar"; // thay port nếu khác
            });
        // Nếu muốn fallback Bearer (paste token thủ công)
        // .AddHttpAuthentication("bearer", auth => { auth.Token = "paste-jwt-here"; });
    });

    // Optional: Swagger UI fallback (nếu Scalar chưa ổn)
    app.UseSwagger();
    app.UseSwaggerUI(opt =>
    {
        opt.SwaggerEndpoint("/swagger/v1/swagger.json", "Order System API v1");
        opt.OAuthClientId("swagger-ui");
        opt.OAuthAppName("Swagger UI");
        opt.OAuthUsePkce();
        opt.OAuthScopes("openid", "profile", audience);
    });
}

app.Run();