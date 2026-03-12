using order_system_modular_monolith.BuildingBlocks.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace order_system_modular_monolith.Product.Exceptions
{
    public class NotFoundProductException : NotFoundException
    {
        public NotFoundProductException(string entity) : base(entity)
        {
            entity = "Products";
        }

    }
}
