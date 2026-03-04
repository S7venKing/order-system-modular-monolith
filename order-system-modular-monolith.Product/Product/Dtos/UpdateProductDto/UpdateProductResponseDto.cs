using order_system_modular_monolith.BuildingBlocks.Application;
using order_system_modular_monolith.BuildingBlocks.CQRS;

namespace order_system_modular_monolith.Product.Dtos.UpdateProductDto
{
    public class UpdateProductResponseDto : Result, ICommand<UpdateProductRequestDto>
    {
        protected UpdateProductResponseDto(bool isSuccess, Error? error) : base(isSuccess, error)
        {
        }

        public string Code { get; set; }
    }
}
