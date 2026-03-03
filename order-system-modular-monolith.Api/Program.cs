using order_system_modular_monolith.Api.Extension;
using order_system_modular_monolith.BuildingBlocks.Infrastructure;
using order_system_modular_monolith.Identity.Extensions.Infrastructure;
using order_system_modular_monolith.Product.Extensions.Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddBuildingBlocksInfrastructure();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "Order System API",
        Version = "v1"
    });
});
builder.AddProductModules(builder.Configuration);
builder.AddIdentityModules(builder.Configuration);
builder.AddSharedInfrastructure();

var app = builder.Build();


app.UseProductModules();
app.UseIdentityModules();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseHttpsRedirection();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference(options =>
    {
        options.OpenApiRoutePattern = "/swagger/{documentName}/swagger.json";
        options.Title = "Order System API";
        options.Theme = ScalarTheme.DeepSpace;
    });
    app.UseSwagger();
    app.UseSwaggerUI();

}

app.Run();
