using StackExchange.Redis;
using System.Text.Json;

namespace order_system_modular_monolith.BuildingBlocks.Infrastructure
{
    /// <summary>
    /// Lightweight helper around StackExchange.Redis for JSON serialization helpers and common operations.
    /// </summary>
    public class RedisHelper
    {
        private readonly IConnectionMultiplexer _connection;

        public RedisHelper(IConnectionMultiplexer connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        private IDatabase Db => _connection.GetDatabase();

        /// <summary>
        /// Set an object as JSON into Redis.
        /// </summary>
        public Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            var json = JsonSerializer.Serialize(value);
            return Db.StringSetAsync(key, json, expiry);
        }

        /// <summary>
        /// Get an object from Redis and deserialize from JSON. Returns default when key missing or deserialization fails.
        /// </summary>
        public async Task<T?> GetAsync<T>(string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            var val = await Db.StringGetAsync(key).ConfigureAwait(false);
            if (!val.HasValue) return default;

            try
            {
                return JsonSerializer.Deserialize<T>(val!.ToString());
            }
            catch
            {
                // swallow deserialization errors and return default to keep helper resilient
                return default;
            }
        }

        /// <summary>
        /// Delete a key.
        /// </summary>
        public Task<bool> RemoveAsync(string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            return Db.KeyDeleteAsync(key);
        }

        /// <summary>
        /// Push an item to the left of a Redis list. The item is stored as JSON string.
        /// </summary>
        public Task<long> ListLeftPushAsync<T>(string key, T item)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            var json = JsonSerializer.Serialize(item);
            return Db.ListLeftPushAsync(key, json);
        }

        /// <summary>
        /// Read all items from a Redis list and deserialize them.
        /// </summary>
        public async Task<List<T>> ListRangeAsync<T>(string key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            var values = await Db.ListRangeAsync(key).ConfigureAwait(false);
            var result = new List<T>(values.Length);
            foreach (var v in values)
            {
                try
                {
                    var item = JsonSerializer.Deserialize<T>(v!.ToString());
                    if (item != null) result.Add(item);
                }
                catch
                {
                    // ignore malformed items
                }
            }
            return result;
        }

        /// <summary>
        /// Remove occurrences of a JSON-serialized value from a list. Returns number of removed items.
        /// </summary>
        public Task<long> ListRemoveAsync<T>(string key, T item, long count = 0)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            var json = JsonSerializer.Serialize(item);
            return Db.ListRemoveAsync(key, json, count);
        }

        /// <summary>
        /// Enumerate keys matching pattern across servers. Pattern examples: "cart:*".
        /// NOTE: This may be slow on large datasets.
        /// </summary>
        public IEnumerable<string> Keys(string pattern)
        {
            var endpoints = _connection.GetEndPoints();
            foreach (var ep in endpoints)
            {
                var server = _connection.GetServer(ep);
                foreach (var key in server.Keys(pattern: pattern))
                {
                    yield return key;
                }
            }
        }
    }
}
