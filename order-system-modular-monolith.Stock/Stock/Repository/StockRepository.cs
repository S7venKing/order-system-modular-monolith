using Microsoft.EntityFrameworkCore;
using order_system_modular_monolith.BuildingBlocks.Exceptions;
using order_system_modular_monolith.Stock.Data;
using order_system_modular_monolith.Stock.Dtos.UpdateStockDto;
using order_system_modular_monolith.Stock.Exceptions;
using order_system_modular_monolith.Stock.Models;
using order_system_modular_monolith.Stock.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace order_system_modular_monolith.Stock.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly StockDbContext _dbContext;

        public StockRepository(StockDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> AddProductQuantity(UpdateStockRequestDto req)
        {
            try
            {
                var affectedRows = await _dbContext.Stocks
                                        .Where(x => x.ProductCode == req.ProductCode)
                                        .ExecuteUpdateAsync(setters => setters
                                        .SetProperty(
                                            p => p.Quantity,
                                            p => p.Quantity + req.Quantity
                                        ));

                if (affectedRows == 0)
                    throw new NotFoundException("ProductCode");

                return "Added successfully";
            }
            catch (Exception) { throw; }

        }

        public async Task<string> RemoveProductQuantity(UpdateStockRequestDto req)
        {
            var stock = await _dbContext.Stocks
                .FirstOrDefaultAsync(x => x.ProductCode == req.ProductCode);

            if (stock == null)
                throw new NotFoundException("ProductCode");

            if (stock.Quantity < req.Quantity)
                throw new NotEnoughStockException(req.ProductCode);

            stock.Quantity -= req.Quantity;

            await _dbContext.SaveChangesAsync();

            return "Removed successfully";
        }

        public async Task<string> UpdateByProductCode(UpdateStockRequestDto req)
        {
            try
            {
                var affectedRows = await _dbContext.Stocks
                    .Where(x => x.ProductCode == req.ProductCode)
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(p => p.Quantity, req.Quantity)
                        );

                if (affectedRows == 0)
                {
                    throw new NotFoundException("ProductCode");
                }

                return req.ProductCode;
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
