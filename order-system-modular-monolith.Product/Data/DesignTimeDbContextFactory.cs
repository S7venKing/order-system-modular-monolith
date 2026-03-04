using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using order_system_modular_monolith.Product.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace order_system_modular_monolith.Module.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ProductDbContext>
    {
        public ProductDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ProductDbContext>();

            builder.UseNpgsql("Host=localhost;Port=5432;Database=ordersdb;Username=postgres;Password=postgres");
            return new ProductDbContext(builder.Options);
        }
    }
}
