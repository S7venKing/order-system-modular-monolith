using order_system_modular_monolith.Product.Models;
using order_system_modular_monolith.Product.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace order_system_modular_monolith.Product.Product.Service
{
    public class ProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public void CreateProduct(string name, decimal price, string category)
        {
            var product = new Products(Guid.NewGuid())
            {
                Name = name,
                Category = category,
                Price = price,
            };
            _productRepository.AddAsync(product);
        }
    }
}
