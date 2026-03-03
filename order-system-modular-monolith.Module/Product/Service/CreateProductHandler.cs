using MediatR;
using order_system_modular_monolith.Product.Models;
using order_system_modular_monolith.Product.Product.Dtos;
using order_system_modular_monolith.Product.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace order_system_modular_monolith.Product.Product.Service
{
    public class CreateProductHandler : IRequestHandler<CreateProductRequestDto, CreateProductResponseDto>
    {
        private readonly IProductRepository _productRepository;

        public CreateProductHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<CreateProductResponseDto> Handle(
            CreateProductRequestDto request,
            CancellationToken cancellationToken)
        {
            var product = new Products(Guid.NewGuid());
            product.Name = request.Name;
            product.Price = request.Price;
            product.Category = request.Category;
            await _productRepository.AddAsync(product);

            return new CreateProductResponseDto
            {
                Id = product.Id
            };
        }
    }
}
