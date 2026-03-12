using MediatR;
using Microsoft.AspNetCore.Http;
using order_system_modular_monolith.Order.Order.Domain;
using order_system_modular_monolith.Order.Order.Infrastructure.Redis;
using System.Collections.Generic;

namespace order_system_modular_monolith.Order.Order.Handlers
{
    public record GetCartQuery() : IRequest<IEnumerable<CartItem>>;

    public class GetCartHandler : IRequestHandler<GetCartQuery, IEnumerable<CartItem>>
    {
        private readonly CartRedisRepository _repo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetCartHandler(CartRedisRepository repo, IHttpContextAccessor httpContextAccessor)
        {
            _repo = repo;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<CartItem>> Handle(GetCartQuery request, CancellationToken cancellationToken)
        {
            var nameId = _httpContextAccessor?.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(nameId)) throw new UnauthorizedAccessException();

            var items = await _repo.GetCartAsync(nameId);
            return items;
        }
    }
}
