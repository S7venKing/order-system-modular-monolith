using System;
using System.Collections.Generic;
using System.Text;
using static order_system_modular_monolith.BuildingBlocks.Domain.Behaviors;

namespace order_system_modular_monolith.BuildingBlocks.Domain
{
    public abstract class AuditableAggregateRoot<TId>
        : AggregateRoot<TId>, IAuditable
        where TId : StronglyTypedId
    {
        public DateTime CreatedAt { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public long? LastModifiedBy { get; set; }

        protected AuditableAggregateRoot(TId id) : base(id) { }
    }
}
