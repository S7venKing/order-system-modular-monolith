using MediatR;
using Microsoft.AspNetCore.Http;
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
        private readonly order_system_modular_monolith.Product.Protos.ProductService.ProductServiceClient _productClient;
        private readonly order_system_modular_monolith.Stock.Protos.StockService.StockServiceClient _stockClient;
        private readonly string _userId;

        public AddToCartHandler(CartRedisRepository repo,
            order_system_modular_monolith.Product.Protos.ProductService.ProductServiceClient productClient,
            order_system_modular_monolith.Stock.Protos.StockService.StockServiceClient stockClient,
            IHttpContextAccessor httpContextAccessor)
        {
            _repo = repo;
            _productClient = productClient;
            _stockClient = stockClient;
            var nameId = httpContextAccessor?.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            _userId = nameId ?? string.Empty;
        }

        public async Task<Unit> Handle(AddToCartCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(_userId)) throw new UnauthorizedAccessException();

            // fetch product
            var prod = await _productClient.GetProductAsync(new GetProductRequest { Code = request.Item.ProductCode });
            if (!prod.Found) throw new KeyNotFoundException("Product not found");

            // fetch stock
            //var stock = await _stockClient.GetStockAsync(new GetStockRequest { Code = request.Item.ProductCode });
            //if (!stock.Found || stock.Quantity <= 0)
            //{
            //    // ensure remove from cart if exists
            //    await _repo.RemoveProductFromCartAsync(_userId, request.Item.ProductCode);
            //    throw new InvalidOperationException("Out of stock");
            //}

            // set product price and name
            request.Item.Price = (decimal)prod.Price;
            request.Item.Name = prod.Name;

            await _repo.AddToCartAsync(_userId, request.Item);

            return Unit.Value;
        }
    }
}
