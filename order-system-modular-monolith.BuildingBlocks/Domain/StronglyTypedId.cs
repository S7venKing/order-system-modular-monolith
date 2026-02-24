using System;
using System.Collections.Generic;
using System.Text;

namespace order_system_modular_monolith.BuildingBlocks.Domain
{
    public abstract record StronglyTypedId(Guid Value)
    {
        public override string ToString() => Value.ToString();
    }
}
