using MediatR;
using order_system_modular_monolith.BuildingBlocks.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace order_system_modular_monolith.BuildingBlocks.Infrastructure
{

    public sealed class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IPublisher _publisher;

        public DomainEventDispatcher(IPublisher publisher)
        {
            _publisher = publisher;
        }

        public async Task DispatchAsync(IReadOnlyList<IDomainEvent> domainEvents)
        {
            foreach (var domainEvent in domainEvents)
            {
                await _publisher.Publish(domainEvent);
            }
        }
    }
}
