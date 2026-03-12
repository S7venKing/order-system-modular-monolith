using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using order_system_modular_monolith.Order.Order.Domain;
using order_system_modular_monolith.BuildingBlocks.Infrastructure;

namespace order_system_modular_monolith.Order.Order.Infrastructure.Redis
{
    public class CartRedisRepository
    {
        private readonly RedisHelper _redis;
        private const string CartKeyPrefix = "cart:";

        public CartRedisRepository(RedisHelper redis)
        {
            _redis = redis ?? throw new ArgumentNullException(nameof(redis));
        }

        private string Key(string userId) => CartKeyPrefix + userId;

        public async Task AddToCartAsync(string userId, CartItem item)
        {
            // We store list of items; push on left for simplicity
            await _redis.ListLeftPushAsync(Key(userId), item);
        }

        public async Task<IEnumerable<CartItem>> GetCartAsync(string userId)
        {
            var items = await _redis.ListRangeAsync<CartItem>(Key(userId));
            return items;
        }

        public async Task RemoveProductFromCartAsync(string userId, string productCode)
        {
            var items = await GetCartAsync(userId);
            var toRemove = items.Where(i => i.ProductCode == productCode).ToList();
            foreach (var item in toRemove)
            {
                await _redis.ListRemoveAsync(Key(userId), item);
            }
        }

        public void RemoveProductFromAllCarts(string productCode)
        {
            // iterate all keys cart:*
            foreach (var key in _redis.Keys("cart:*"))
            {
                // key is RedisKey-like; we need to trim prefix when calling repo methods
                var userId = key.StartsWith(CartKeyPrefix) ? key.Substring(CartKeyPrefix.Length) : key;
                // fire-and-forget removal
#pragma warning disable CS4014
                RemoveProductFromCartAsync(userId, productCode);
#pragma warning restore CS4014
            }
        }
    }
}
