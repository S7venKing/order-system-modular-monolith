using order_system_modular_monolith.BuildingBlocks.Application.Abstractions;
using order_system_modular_monolith.Stock.Dtos.UpdateStockDto;
using order_system_modular_monolith.Stock.Models;

namespace order_system_modular_monolith.Stock.Repository
{
    public interface IStockRepository
    {
        Task<string> UpdateByProductCode(UpdateStockRequestDto req);

        Task<string> AddProductQuantity(UpdateStockRequestDto req);

        Task<string> RemoveProductQuantity(UpdateStockRequestDto req);

    }
}
