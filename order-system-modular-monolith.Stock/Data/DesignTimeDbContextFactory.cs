using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using order_system_modular_monolith.Stock.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace order_system_modular_monolith.Module.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<StockDbContext>
    {
        public StockDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<StockDbContext>();

            builder.UseNpgsql("Host=localhost;Port=5432;Database=ordersdb;Username=postgres;Password=postgres");
            return new StockDbContext(builder.Options);
        }
    }
}
