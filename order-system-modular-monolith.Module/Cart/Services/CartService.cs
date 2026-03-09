using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using order_system_modular_monolith.BuildingBlocks.Web;
using order_system_modular_monolith.Cart.Dtos;

namespace order_system_modular_monolith.Cart.Services
{
    public class CartService
    {
        private readonly IDistributedCache _cache;
        private readonly ICurrentUserProvider _currentUser;
        private const string KEY_PREFIX = "cart:";

        public CartService(IDistributedCache cache, ICurrentUserProvider currentUser)
        {
            _cache = cache;
            _currentUser = currentUser;
        }

        private string GetKey()
        {
            var userId = _currentUser.GetCurrentUserId();
            if (userId == null)
                throw new InvalidOperationException("Authentication required to access cart.");

            return KEY_PREFIX + userId.Value;
        }

        public async Task<List<CartItemDto>> GetCartAsync()
        {
            var key = GetKey();
            var data = await _cache.GetStringAsync(key);
            if (string.IsNullOrEmpty(data))
                return new List<CartItemDto>();

            return JsonSerializer.Deserialize<List<CartItemDto>>(data) ?? new List<CartItemDto>();
        }

        public async Task SaveCartAsync(List<CartItemDto> items)
        {
            var key = GetKey();
            var data = JsonSerializer.Serialize(items);
            await _cache.SetStringAsync(key, data);
        }

        public async Task AddOrUpdateItemAsync(CartItemDto item)
        {
            var items = await GetCartAsync();
            var existing = items.FirstOrDefault(x => x.ProductCode == item.ProductCode);
            if (existing != null)
            {
                // lock by version: only update if incoming version matches stored version
                if (item.Version <= existing.Version)
                {
                    // older or same version => ignore
                    return;
                }
                items.Remove(existing);
            }

            items.Add(item);
            await SaveCartAsync(items);
        }

        public async Task RemoveItemAsync(string productCode)
        {
            var items = await GetCartAsync();
            var existing = items.FirstOrDefault(x => x.ProductCode == productCode);
            if (existing != null)
            {
                items.Remove(existing);
                await SaveCartAsync(items);
            }
        }
    }
}
