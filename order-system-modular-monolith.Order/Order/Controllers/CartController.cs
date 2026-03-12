using Microsoft.AspNetCore.Mvc;
using order_system_modular_monolith.Order.Order.Domain;
using order_system_modular_monolith.Order.Order.Infrastructure.Redis;
using order_system_modular_monolith.BuildingBlocks.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace order_system_modular_monolith.Order.Order.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class CartController : ControllerBase
    {
        private readonly CartRedisRepository _repo;
        private readonly ICurrentUserProvider _currentUserProvider;

        public CartController(CartRedisRepository repo, ICurrentUserProvider currentUserProvider)
        {
            _repo = repo;
            _currentUserProvider = currentUserProvider;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromBody] CartItem item)
        {
            var userId = _currentUserProvider.GetCurrentUserId();
            if (userId == null)
                return Unauthorized();

            await _repo.AddToCartAsync(userId.Value.ToString(), item);
            return Ok();
        }

        [HttpGet("getcart")]
        public async Task<IActionResult> GetCart()
        {
            var userId = _currentUserProvider.GetCurrentUserId();
            if (userId == null)
                return Unauthorized();

            var cart = await _repo.GetCartAsync(userId.Value.ToString());
            return Ok(cart);
        }
    }
}
