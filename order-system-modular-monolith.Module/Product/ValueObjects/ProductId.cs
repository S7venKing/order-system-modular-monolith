using order_system_modular_monolith.BuildingBlocks.Domain;

namespace order_system_modular_monolith.Module.Product.ValueObjects
{
    public record ProductId(Guid Value) : StronglyTypedId(Value)
    {

    }
}
