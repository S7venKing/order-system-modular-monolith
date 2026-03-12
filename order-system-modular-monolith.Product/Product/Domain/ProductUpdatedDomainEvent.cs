using order_system_modular_monolith.BuildingBlocks.Domain;

namespace order_system_modular_monolith.Product.Domain
{
    public record ProductUpdatedDomainEvent(Guid ProductId, string ProductCode, string Name, decimal Price, string Category, long Version)
        : DomainEvent
    {

    }
}
