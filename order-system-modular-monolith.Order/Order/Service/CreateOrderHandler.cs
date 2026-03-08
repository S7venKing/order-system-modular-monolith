using MediatR;
using order_system_modular_monolith.Order.Order.Dtos;
using order_system_modular_monolith.Order.Repository;
using order_system_modular_monolith.BuildingBlocks.Application;

namespace order_system_modular_monolith.Order.Order.Service
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderRequestDto, CreateOrderResponseDto>
    {
        private readonly IOrderRepository _orderRepository;
        public CreateOrderHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public async Task<CreateOrderResponseDto> Handle(CreateOrderRequestDto request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _orderRepository.CreateOrder(request);
                return CreateOrderResponseDto.Success();
            }
            catch (Exception)
            {
                return CreateOrderResponseDto.Failure(new Error("500", "Error"));
            }
        }
    }
}
