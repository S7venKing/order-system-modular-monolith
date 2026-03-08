using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using order_system_modular_monolith.Order.Order.Dtos;
using order_system_modular_monolith.Order.Order.Service;
namespace order_system_modular_monolith.Order.Order.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly CreateOrderHandler _createOrderHandler;
        public OrderController(CreateOrderHandler createOrderHandler)
        {
            _createOrderHandler = createOrderHandler;
        }
        [HttpPost("create")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequestDto input, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _createOrderHandler.Handle(input, cancellationToken);
                if (response == null || !response.IsSuccess)
                {
                    return BadRequest("Create Order failed! " + response?.Error?.Message);
                }
                return Ok("Create Order successfully");
            }
            catch (Exception)
            {
                return BadRequest("Error");
            }
        }
    }
}
