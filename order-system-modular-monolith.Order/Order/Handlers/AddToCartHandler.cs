using MediatR;
using Microsoft.AspNetCore.Http;
using order_system_modular_monolith.BuildingBlocks.Infrastructure;
using order_system_modular_monolith.Order.Order.Domain;
using order_system_modular_monolith.Order.Order.Infrastructure.Redis;
using order_system_modular_monolith.Product.Protos;
using order_system_modular_monolith.Stock.Protos;

namespace order_system_modular_monolith.Order.Order.Handlers
{
    public record AddToCartCommand(CartItem Item) : IRequest<Unit>;

    public class AddToCartHandler : IRequestHandler<AddToCartCommand, Unit>
    {
        private readonly CartRedisRepository _repo;
        private readonly RedisHelper _redis;
        private readonly ProductService.ProductServiceClient _productClient;
        private readonly StockService.StockServiceClient _stockClient;
        private readonly string _userId;

        public AddToCartHandler(
            CartRedisRepository repo,
            RedisHelper redis,
            ProductService.ProductServiceClient productClient,
            StockService.StockServiceClient stockClient,
            IHttpContextAccessor httpContextAccessor)
        {
            _repo = repo;
            _redis = redis;
            _productClient = productClient;
            _stockClient = stockClient;

            // IdentityServer access token usually contains `sub` as the stable subject id.
            // Fallback to NameIdentifier for cases where the claim is emitted as-is.
            var userId =
                httpContextAccessor?.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                ?? httpContextAccessor?.HttpContext?.User?.FindFirst("sub")?.Value;

            _userId = userId ?? string.Empty;
        }

        public async Task<Unit> Handle(AddToCartCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(_userId))
                throw new UnauthorizedAccessException();

            var productCode = request.Item.ProductCode;

            var lockKey = $"lock:cart:{_userId}:{productCode}";
            var lockValue = Guid.NewGuid().ToString();

            var locked = await _redis.LockTakeAsync(lockKey, lockValue, TimeSpan.FromSeconds(5));

            if (!locked)
                throw new Exception("Another add-to-cart request is processing");

            try
            {
                // get product
                var prod = await _productClient.GetProductAsync(
                    new GetProductRequest { Code = productCode });

                if (!prod.Found)
                    throw new KeyNotFoundException("Product not found");

                // get stock
                var stock = await _stockClient.GetStockAsync(
                    new GetStockRequest { Code = productCode });

                if (!stock.Found || stock.Quantity <= 0)
                {
                    await _repo.RemoveProductFromCartAsync(_userId, productCode);
                    throw new InvalidOperationException("Out of stock");
                }

                request.Item.Price = (decimal)prod.Price;
                request.Item.Name = prod.Name;
        
                await _repo.AddToCartAsync(_userId, request.Item);

                return Unit.Value;
            }
            finally
            {
                await _redis.LockReleaseAsync(lockKey, lockValue);
            }
        }
    }
}