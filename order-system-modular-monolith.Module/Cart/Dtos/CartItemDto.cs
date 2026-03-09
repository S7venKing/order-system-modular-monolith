namespace order_system_modular_monolith.Cart.Dtos
{
    public record CartItemDto(string ProductCode, string Name, decimal Price, int Quantity, long Version);
}
