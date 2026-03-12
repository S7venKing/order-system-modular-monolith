using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using order_system_modular_monolith.Product.Data;
using order_system_modular_monolith.Product.Protos;

namespace order_system_modular_monolith.Product.Grpc
{
    public class ProductServiceImpl : ProductService.ProductServiceBase
    {
        private readonly ProductDbContext _db;

        public ProductServiceImpl(ProductDbContext db)
        {
            _db = db;
        }

        public override async Task<GetProductResponse> GetProduct(GetProductRequest request, ServerCallContext context)
        {
            var prod = await _db.Products.FirstOrDefaultAsync(p => p.Code == request.Code, context.CancellationToken);
            if (prod == null)
            {
                return new GetProductResponse { Found = false };
            }

            return new GetProductResponse
            {
                Code = prod.Code,
                Name = prod.Name,
                Price = (double)prod.Price,
                Found = true
            };
        }
    }
}
