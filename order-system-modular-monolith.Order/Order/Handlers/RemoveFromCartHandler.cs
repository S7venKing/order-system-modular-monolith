using MediatR;
using Microsoft.AspNetCore.Http;
using order_system_modular_monolith.Order.Order.Infrastructure.Redis;

namespace order_system_modular_monolith.Order.Order.Handlers
{
    public record RemoveFromCartCommand(string ProductCode) : IRequest<Unit>;

    public class RemoveFromCartHandler : IRequestHandler<RemoveFromCartCommand, Unit>
    {
        private readonly CartRedisRepository _repo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RemoveFromCartHandler(CartRedisRepository repo, IHttpContextAccessor httpContextAccessor)
        {
            _repo = repo;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Unit> Handle(RemoveFromCartCommand request, CancellationToken cancellationToken)
        {
            var nameId = _httpContextAccessor?.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(nameId)) throw new UnauthorizedAccessException();

            await _repo.RemoveProductFromCartAsync(nameId, request.ProductCode);
            return Unit.Value;
        }
    }
}
