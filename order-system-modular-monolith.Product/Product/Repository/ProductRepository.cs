using Microsoft.EntityFrameworkCore;
using order_system_modular_monolith.Product.Data;
using order_system_modular_monolith.Product.Dtos.UpdateProductDto;
using order_system_modular_monolith.Product.Models;
using order_system_modular_monolith.Product.Product.Exceptions;
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
            try
            {
                var product = _dbContext.Products.FirstOrDefault(a => a.Code == entity.Code);
                if (product != null) { throw new ExistingException("Existing product with this code"); }
                await _dbContext.Products.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task Remove(Products entity)
        {
            _dbContext.Products.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Update(Products entity)
        {
            _dbContext.Products.Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<string> UpdateByCode(UpdateProductRequestDto req)
        {
            try
            {
                var affectedRows = await _dbContext.Products
                    .Where(x => x.Code == req.Code)
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(p => p.Name, req.Name)
                        .SetProperty(p => p.Price, req.Price)
                        .SetProperty(p => p.Category, req.Category)
                        );

                if (affectedRows == 0)
                {
                    throw new NotFoundProductException("Products");
                }
                return req.Code;
            }
            catch (Exception) {
                throw ;
            }

        }
    }
}
