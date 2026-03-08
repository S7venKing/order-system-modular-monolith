using MediatR;

namespace order_system_modular_monolith.Order.Order.Dtos
{
    public class CreateOrderRequestDto : IRequest<CreateOrderResponseDto>
    {
        public string OrderNumber { get; set; } = default!;
        public decimal Total { get; set; }
    }
}
