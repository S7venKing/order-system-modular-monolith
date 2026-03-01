using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using order_system_modular_monolith.BuildingBlocks.Domain;
using order_system_modular_monolith.BuildingBlocks.EFCore;
using order_system_modular_monolith.BuildingBlocks.Infrastructure;

namespace order_system_modular_monolith.Product.Data
{
    public class ProductDbContext : DbContext, IDbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options)
            : base(options) { }

        public const string Schema = "products";

        public DbSet<Product.Models.Product> Products => Set<Product.Models.Product>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schema);
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(ProductDbContext).Assembly);
        }

        public IReadOnlyList<IDomainEvent> GetDomainEvents()
        {
            throw new NotImplementedException();
        }

        public Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public IExecutionStrategy CreateExecutionStrategy()
        {
            throw new NotImplementedException();
        }

        public Task ExecuteTransactionalAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
