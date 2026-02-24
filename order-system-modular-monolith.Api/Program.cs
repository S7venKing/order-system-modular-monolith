using order_system_modular_monolith.BuildingBlocks.Infrastructure;
using order_system_modular_monolith.Api.Extension;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddBuildingBlocksInfrastructure();
builder.AddSharedInfrastructure();

var app = builder.Build();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
