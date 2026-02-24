using System;
using System.Collections.Generic;
using System.Text;

namespace order_system_modular_monolith.BuildingBlocks.Domain
{
    public interface IDomainEvent
    {
        Guid Id { get; }
        DateTime OccurredOnUtc { get; }
    }
}
