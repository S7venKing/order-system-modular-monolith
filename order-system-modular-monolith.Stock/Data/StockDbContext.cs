using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using order_system_modular_monolith.BuildingBlocks.Application.Abstractions;
using order_system_modular_monolith.BuildingBlocks.Domain;
using order_system_modular_monolith.BuildingBlocks.EFCore;
using order_system_modular_monolith.BuildingBlocks.Infrastructure;
using order_system_modular_monolith.BuildingBlocks.Web;
using order_system_modular_monolith.Stock.Models;

namespace order_system_modular_monolith.Stock.Data
{
    public class StockDbContext : AppDbContextBase<StockDbContext>
    {
        private readonly ICurrentUserProvider? _currentUserProvider;
        private readonly ILogger<AppDbContextBase<StockDbContext>>? _logger;
        private IDbContextTransaction _currentTransaction;
        private readonly IDateTimeProvider _dateTimeProvider;

        public const string Schema = "stocks";

        public StockDbContext(DbContextOptions<StockDbContext> options, ICurrentUserProvider? currentUserProvider = null, ILogger<AppDbContextBase<StockDbContext>>? logger = null, IDateTimeProvider? dateTimeProvider = null) : base(options, currentUserProvider, logger, dateTimeProvider)
        {
        }


        public DbSet<Stocks> Stocks => Set<Stocks>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schema);
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(StockDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
