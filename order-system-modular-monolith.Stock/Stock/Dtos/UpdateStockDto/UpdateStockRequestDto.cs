using MediatR;
using System.ComponentModel.DataAnnotations;

namespace order_system_modular_monolith.Stock.Dtos.UpdateStockDto
{
    public class UpdateStockRequestDto : IRequest<UpdateStockResponseDto>
    {
        [Required(ErrorMessage = "Product Code is required")]
        public string ProductCode { get; set; }

        [Required(ErrorMessage = "Stock name is required")]
        public decimal Quantity { get; set; }
    }
}
