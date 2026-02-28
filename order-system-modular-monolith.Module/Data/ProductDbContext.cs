using Microsoft.EntityFrameworkCore;
using order_system_modular_monolith.BuildingBlocks.Infrastructure;

namespace order_system_modular_monolith.Module.Product.Data
{
    public class ProductDbContext : DbContext
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
        
    }
}
