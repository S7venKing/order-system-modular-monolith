using System;
using System.Collections.Generic;
using System.Text;

namespace order_system_modular_monolith.BuildingBlocks.Domain
{
    public abstract class AggregateRoot<TId>
        : Entity<TId>, IHasDomainEvents

    {
        private readonly List<IDomainEvent> _domainEvents = new();

        protected AggregateRoot(TId id) : base(id) { }

        protected AggregateRoot() { }


        public IReadOnlyCollection<IDomainEvent> DomainEvents
            => _domainEvents.AsReadOnly();

        protected void Raise(IDomainEvent domainEvent)
            => _domainEvents.Add(domainEvent);

        public void ClearDomainEvents()
            => _domainEvents.Clear();
    }
}
