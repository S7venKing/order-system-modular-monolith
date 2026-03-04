using order_system_modular_monolith.BuildingBlocks.Domain;

namespace order_system_modular_monolith.Module.Stock.ValueObjects
{
    public record StockId(Guid Value) : StronglyTypedId(Value)
    {

    }
}
