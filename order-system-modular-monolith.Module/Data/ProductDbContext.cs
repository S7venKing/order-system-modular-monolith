using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using order_system_modular_monolith.BuildingBlocks.Domain;
using order_system_modular_monolith.BuildingBlocks.EFCore;
using order_system_modular_monolith.BuildingBlocks.Infrastructure;
using order_system_modular_monolith.Product.Models;

namespace order_system_modular_monolith.Product.Data
{
    public class ProductDbContext : AppDbContextBase
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options)
            : base(options) { }

        public const string Schema = "products";

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
