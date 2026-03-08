using order_system_modular_monolith.BuildingBlocks.Application;

namespace order_system_modular_monolith.Order.Order.Dtos
{
    public class CreateOrderResponseDto : Result
    {
        protected CreateOrderResponseDto(bool isSuccess, Error? error) : base(isSuccess, error)
        {
        }

        public static CreateOrderResponseDto Success() => new CreateOrderResponseDto(true, null);
        public static CreateOrderResponseDto Failure(Error error) => new CreateOrderResponseDto(false,error);
    }
}
