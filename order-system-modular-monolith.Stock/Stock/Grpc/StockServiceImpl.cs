using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using order_system_modular_monolith.Stock.Data;
using order_system_modular_monolith.Stock.Protos;

namespace order_system_modular_monolith.Stock.Stock.Grpc
{
    public class StockServiceImpl : StockService.StockServiceBase
    {
        private readonly StockDbContext _db;

        public StockServiceImpl(StockDbContext db)
        {
            _db = db;
        }

        public override async Task<GetStockResponse> GetStock(GetStockRequest request, ServerCallContext context)
        {
            var stock = await _db.Stocks.FirstOrDefaultAsync(s => s.ProductCode == request.Code, context.CancellationToken);
            if (stock == null)
                return new GetStockResponse { Found = false };

            return new GetStockResponse
            {
                Code = stock.ProductCode,
                Quantity = (double)stock.Quantity,
                Found = true
            };
        }
    }
}
