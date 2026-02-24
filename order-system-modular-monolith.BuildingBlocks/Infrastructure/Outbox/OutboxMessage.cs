using System;
using System.Collections.Generic;
using System.Text;

namespace order_system_modular_monolith.BuildingBlocks.Infrastructure.Outbox
{
    public class OutboxMessage
    {
        public Guid Id { get; set; }
        public DateTime OccurredOn { get; set; }
        public string Type { get; set; } = default!;
        public string Payload { get; set; } = default!;
        public bool Processed { get; set; }
    }
}
