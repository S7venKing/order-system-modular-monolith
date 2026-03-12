using MediatR;
using order_system_modular_monolith.Product.Models;
using order_system_modular_monolith.Product.Dtos.UpdateProductDto;
using order_system_modular_monolith.Product.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using order_system_modular_monolith.BuildingBlocks.Application;
using order_system_modular_monolith.Product.Exceptions;

namespace order_system_modular_monolith.Product.Product.Service
{
    public class UpdateProductHandler : IRequestHandler<UpdateProductRequestDto, UpdateProductResponseDto>
    {
        private readonly IProductRepository _productRepository;

        public UpdateProductHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<UpdateProductResponseDto> Handle(UpdateProductRequestDto request, CancellationToken cancellationToken)
        {
            try
            {
                var code = await _productRepository.UpdateByCode(request);

                return (UpdateProductResponseDto)UpdateProductResponseDto.Success();
            }
            catch (NotFoundProductException ex)
            {
                return (UpdateProductResponseDto)UpdateProductResponseDto.Failure(new Error("404", ex.Message));
            }
            catch (Exception)
            {
                return (UpdateProductResponseDto)UpdateProductResponseDto.Failure(new Error("500", "Có lỗi xảy ra"));
            }
        }
    }
}
