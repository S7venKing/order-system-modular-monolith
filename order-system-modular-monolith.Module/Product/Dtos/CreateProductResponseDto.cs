using order_system_modular_monolith.BuildingBlocks.CQRS;
using System;
using System.Collections.Generic;
using System.Text;

namespace order_system_modular_monolith.Product.Product.Dtos
{
    public class CreateProductResponseDto : ICommand<CreateProductRequestDto>
    {
        public Guid Id { get; set; }
    }
}
