using order_system_modular_monolith.BuildingBlocks.Application.Abstractions;
using order_system_modular_monolith.Product.Dtos.UpdateProductDto;
using order_system_modular_monolith.Product.Models;

namespace order_system_modular_monolith.Product.Repository
{
    public interface IProductRepository : IRepository<Products, Guid>
    {
        Task<string> UpdateByCode(UpdateProductRequestDto req);
        Task<Products?> GetByCodeAsync(string code);
    }
}
