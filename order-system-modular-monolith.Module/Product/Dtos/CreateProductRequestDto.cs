using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace order_system_modular_monolith.Product.Product.Dtos
{
    public class CreateProductRequestDto : IRequest<CreateProductResponseDto>
    {
        [Required(ErrorMessage = "Product name is required")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price is required")]
        public long Price { get; set; } = 50;

        public string Category { get; set; } = "normal";
    }
}
