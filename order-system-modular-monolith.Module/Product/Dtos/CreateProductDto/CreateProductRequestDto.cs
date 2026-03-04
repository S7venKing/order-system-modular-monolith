using MediatR;
using System.ComponentModel.DataAnnotations;

namespace order_system_modular_monolith.Product.Product.Dtos.CreateProductDto
{
    public class CreateProductRequestDto : IRequest<CreateProductResponseDto>
    {
        [Required(ErrorMessage = "Product code is required")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price is required")]
        public long Price { get; set; } = 50;
        public string Category { get; set; } = "normal";
    }
}
