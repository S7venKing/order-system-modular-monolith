using Microsoft.EntityFrameworkCore;
using order_system_modular_monolith.Order.Data;
using order_system_modular_monolith.Order.Order.Dtos;
using order_system_modular_monolith.Order.Models;

namespace order_system_modular_monolith.Order.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDbContext _db;

        public OrderRepository(OrderDbContext db)
        {
            _db = db;
        }

        public async Task<string> CreateOrder(CreateOrderRequestDto req)
        {
            var order = new Orders
            {
                OrderNumber = req.OrderNumber,
                Total = req.Total
            };

            _db.Add(order);
            await _db.SaveChangesAsync();
            return order.OrderNumber;
        }
    }
}
