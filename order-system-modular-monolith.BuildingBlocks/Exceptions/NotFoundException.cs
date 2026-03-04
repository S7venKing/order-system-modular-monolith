using System;
using System.Collections.Generic;
using System.Text;

namespace order_system_modular_monolith.BuildingBlocks.Exceptions
{
    public class NotFoundException : AppException
    {
        public NotFoundException(string entity)
            : base("not_found",
                $"{entity} was not found")
        {
        }
    }
}
