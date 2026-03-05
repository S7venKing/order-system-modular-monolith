using order_system_modular_monolith.BuildingBlocks.Domain;

namespace order_system_modular_monolith.Product.Product.Domain
{
    public record ProductCreatedDomainEvent(Guid ProductId, string ProductCode, long Quantity)
        : DomainEvent
    {

    }
}
