using System.Collections.Generic;

namespace order_system_modular_monolith.Order.Order.Domain
{
    public class Cart
    {
        public string UserId { get; set; } = string.Empty;
        public List<CartItem> Items { get; set; } = new List<CartItem>();
    }
}
