using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace order_system_modular_monolith.Product.Product.Dtos
{
    public class CreateProductRequestDto : IRequest<CreateProductResponseDto>
    {
        [JsonIgnore]
        public CancellationToken cancellationToken = default(CancellationToken);
    }
}
