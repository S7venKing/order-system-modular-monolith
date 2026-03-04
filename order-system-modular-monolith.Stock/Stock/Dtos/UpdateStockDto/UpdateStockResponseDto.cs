using order_system_modular_monolith.BuildingBlocks.Application;
using order_system_modular_monolith.BuildingBlocks.CQRS;

namespace order_system_modular_monolith.Stock.Dtos.UpdateStockDto
{
    public class UpdateStockResponseDto : Result, ICommand<UpdateStockRequestDto>
    {
        protected UpdateStockResponseDto(bool isSuccess, Error? error) : base(isSuccess, error)
        {
        }

    }
}
