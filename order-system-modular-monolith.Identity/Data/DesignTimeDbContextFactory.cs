using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using order_system_modular_monolith.Identity.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace order_system_modular_monolith.Module.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<IdentityContext>
    {
        public IdentityContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<IdentityContext>();

            builder.UseNpgsql("Server=localhost;Port=63293;Database=ordersdb;User Id=postgres;Password=ydQ}90jG!Yj2Bg8CnfkDmJ;Include Error Detail=true");
            return new IdentityContext(builder.Options);
        }
    }
}
