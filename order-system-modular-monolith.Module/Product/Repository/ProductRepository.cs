using order_system_modular_monolith.Product.Data;
using order_system_modular_monolith.Product.Models;
using order_system_modular_monolith.Product.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace order_system_modular_monolith.Product.Product.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDbContext _dbContext;

        public ProductRepository(ProductDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Products entity)
        {
            await _dbContext.Products.AddAsync(entity);
        }

        public void Remove(Products entity)
        {
            _dbContext.Products.Remove(entity);
        }

        public void Update(Products entity)
        {
            _dbContext.Products.Update(entity);
        }
    }
}
