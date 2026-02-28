using System;
using System.Collections.Generic;
using System.Text;

namespace order_system_modular_monolith.BuildingBlocks.Domain
{
    public abstract class Entity<TId>
    {
        public abstract TId Id { get; set; }

        protected Entity(TId id)
        {
            Id = id;
        }
    }
}
