using MediatR;
using order_system_modular_monolith.Product.Domain;
using order_system_modular_monolith.Stock.Data;
using order_system_modular_monolith.Stock.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace order_system_modular_monolith.Stock.Service
{
    public class ProductCreatedHandler
        : INotificationHandler<ProductCreatedDomainEvent>
    {
        private readonly StockDbContext _db;

        public ProductCreatedHandler(StockDbContext db)
        {
            _db = db;
        }

        public async Task Handle(
            ProductCreatedDomainEvent notification,
            CancellationToken cancellationToken)
        {
            var stock = new Stocks
            {
                ProductCode = notification.ProductCode,
                Quantity = notification.Quantity,
            };

            _db.Stocks.Add(stock);

            await _db.SaveChangesAsync(cancellationToken);
        }
    }
}
