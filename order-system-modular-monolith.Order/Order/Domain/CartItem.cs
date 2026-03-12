using System;

namespace order_system_modular_monolith.Order.Order.Domain
{
    public class CartItem
    {
        public string ProductCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
