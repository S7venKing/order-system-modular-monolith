using Microsoft.AspNetCore.Mvc;
using MediatR;
using order_system_modular_monolith.Order.Order.Domain;
using order_system_modular_monolith.BuildingBlocks.Web;
using order_system_modular_monolith.Order.Order.Handlers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace order_system_modular_monolith.Order.Order.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CartController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] CartItem item)
        {
            try
            {
                await _mediator.Send(new AddToCartCommand(item));
                return Ok();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getcart")]
        public async Task<IActionResult> GetCart()
        {
            try
            {
                var items = await _mediator.Send(new GetCartQuery());
                return Ok(items);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpDelete("remove/{productCode}")]
        public async Task<IActionResult> RemoveFromCart(string productCode)
        {
            try
            {
                await _mediator.Send(new RemoveFromCartCommand(productCode));
                return Ok();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }
    }
}
