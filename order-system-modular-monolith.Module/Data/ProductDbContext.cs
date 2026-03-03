using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using order_system_modular_monolith.BuildingBlocks.Application.Abstractions;
using order_system_modular_monolith.BuildingBlocks.Domain;
using order_system_modular_monolith.BuildingBlocks.EFCore;
using order_system_modular_monolith.BuildingBlocks.Infrastructure;
using order_system_modular_monolith.BuildingBlocks.Web;
using order_system_modular_monolith.Product.Models;

namespace order_system_modular_monolith.Product.Data
{
    public class ProductDbContext : AppDbContextBase
    {
        private readonly ICurrentUserProvider? _currentUserProvider;
        private readonly ILogger<AppDbContextBase>? _logger;
        private IDbContextTransaction _currentTransaction;
        private readonly IDateTimeProvider _dateTimeProvider;



        public const string Schema = "products";

        public ProductDbContext(DbContextOptions options, ICurrentUserProvider currentUserProvider, ILogger<AppDbContextBase> logger, IDateTimeProvider dateTimeProvider) : base(options, currentUserProvider, logger, dateTimeProvider)
        {
        }

        public ProductDbContext(DbContextOptions<ProductDbContext> options): base(options)
        {
        }

        public DbSet<Products> Products => Set<Products>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schema);
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(ProductDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
