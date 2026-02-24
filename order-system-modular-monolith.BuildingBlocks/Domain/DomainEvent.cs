using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace order_system_modular_monolith.BuildingBlocks.Domain
{

    public abstract record DomainEvent :
    IDomainEvent,
    INotification
    {
        public Guid Id { get; } = Guid.NewGuid();
        public DateTime OccurredOnUtc { get; init; } = DateTime.UtcNow;
    }
}
