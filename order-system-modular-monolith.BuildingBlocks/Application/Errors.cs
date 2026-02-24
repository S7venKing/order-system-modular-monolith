using System;
using System.Collections.Generic;
using System.Text;

namespace order_system_modular_monolith.BuildingBlocks.Application
{
    public record Error(
        string Code,
        string Message);
}
