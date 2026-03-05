using order_system_modular_monolith.BuildingBlocks.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace order_system_modular_monolith.BuildingBlocks.Infrastructure
{
    public interface IDomainEventDispatcher
    {
        Task DispatchAsync(IReadOnlyList<IDomainEvent> domainEvents);
    }
}
