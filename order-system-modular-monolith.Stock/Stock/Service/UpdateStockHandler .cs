using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using order_system_modular_monolith.BuildingBlocks.Application;
using order_system_modular_monolith.Stock.Dtos.UpdateStockDto;
using order_system_modular_monolith.Stock.Repository;
using order_system_modular_monolith.BuildingBlocks.Exceptions;

namespace order_system_modular_monolith.Stock.Service
{
    public class UpdateStockHandler : IRequestHandler<UpdateStockRequestDto, UpdateStockResponseDto>
    {
        private readonly IStockRepository _stockRepository;

        public UpdateStockHandler(IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }

        public async Task<UpdateStockResponseDto> Handle(UpdateStockRequestDto request, CancellationToken cancellationToken)
        {
            try
            {
                var code = await _stockRepository.UpdateByProductCode(request);

                return (UpdateStockResponseDto)UpdateStockResponseDto.Success();
            }
            catch (NotFoundException ex)
            {
                return (UpdateStockResponseDto)UpdateStockResponseDto.Failure(new Error("404", ex.Message));
            }
            catch (Exception)
            {
                return (UpdateStockResponseDto)UpdateStockResponseDto.Failure(new Error("500", "Có lỗi xảy ra"));
            }
        }
    }
}
