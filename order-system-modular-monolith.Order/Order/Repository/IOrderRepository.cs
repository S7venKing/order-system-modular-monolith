using order_system_modular_monolith.BuildingBlocks.Application.Abstractions;
using order_system_modular_monolith.Order.Order.Dtos;

namespace order_system_modular_monolith.Order.Repository
{
    public interface IOrderRepository
    {
        Task<string> CreateOrder(CreateOrderRequestDto req);
    }
}
