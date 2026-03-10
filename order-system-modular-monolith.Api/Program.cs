using Microsoft.OpenApi;
using order_system_modular_monolith.Api.Extension;
using order_system_modular_monolith.BuildingBlocks.Infrastructure;
using order_system_modular_monolith.Identity.Extensions.Infrastructure;
using order_system_modular_monolith.Product.Extensions.Infrastructure;
using order_system_modular_monolith.Stock.Extensions.Infrastructure;
using Prometheus;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddBuildingBlocksInfrastructure();
builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();

var authority = builder.Configuration["Jwt:Authority"] ?? "https://localhost:5001";
var audience = builder.Configuration["Jwt:Audience"] ?? "order-system-modular-monolith";
var authorityUri = new Uri(authority.TrimEnd('/'));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSwagger", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});



builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Order System API", Version = "v1" });

    // Bearer JWT (paste token)
    options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Paste JWT token vào đây. Ví dụ: Bearer eyJhbGciOiJIUzI1NiIs..."
    });

    options.AddSecurityRequirement((document) => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference("bearer", document!),
            new List<string> { }
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

            // Paste JWT token
            .AddHttpAuthentication("bearer", auth =>
            {
                auth.Token = "";
            });
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
    app.MapPrometheusScrapingEndpoint();
    app.UseCors("AllowSwagger");
}

app.Run();