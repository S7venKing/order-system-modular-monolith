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

    // Định nghĩa OAuth2 scheme
    options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "Paste JWT Bearer token (từ login riêng)"
    });

    options.AddSecurityRequirement((document) => new OpenApiSecurityRequirement
{
    {
        new OpenApiSecuritySchemeReference("bearer", document!),
        new List<string>(){} 
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

// Enable authentication before authorization so JWT tokens / IdentityServer are validated
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
            .WithTheme(ScalarTheme.DeepSpace)  // hoặc Moon, Purple, ...
            .WithOpenApiRoutePattern("/swagger/v1/swagger.json")
            .WithHttpBearerAuthentication(bearer => { bearer.Token = "paste-jwt-here-if-needed"; });
    });

    // Optional: Giữ Swagger UI cũ nếu muốn so sánh
    app.UseSwagger();
    app.UseSwaggerUI(opt =>
    {
        opt.SwaggerEndpoint("/swagger/v1/swagger.json", "Order System API v1");
        //opt.OAuthClientId("swagger-ui");
        //opt.OAuthAppName("Swagger UI");
        //opt.OAuthUsePkce();
        //opt.OAuthScopes("openid", "profile", audience);
    });
}

app.Run();
