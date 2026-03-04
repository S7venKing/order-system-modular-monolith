using System;
using System.Collections.Generic;
using System.Text;
using static order_system_modular_monolith.BuildingBlocks.Domain.Behaviors;

namespace order_system_modular_monolith.BuildingBlocks.Domain
{
    public abstract class FullTrackedAggregateRoot<TId>
        : AuditableAggregateRoot<TId>, ISoftDelete, IVersioned
    {
        public bool IsDeleted { get; set; }
        public long Version { get; set; }

        public FullTrackedAggregateRoot(TId id) : base(id) { }

        public FullTrackedAggregateRoot() { }

    }
}
