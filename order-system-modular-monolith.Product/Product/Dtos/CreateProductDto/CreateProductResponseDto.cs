using order_system_modular_monolith.BuildingBlocks.CQRS;

namespace order_system_modular_monolith.Product.Dtos.CreateProductDto
{
    public class CreateProductResponseDto : ICommand<CreateProductRequestDto>
    {
        public Guid Id { get; set; }
    }
}
