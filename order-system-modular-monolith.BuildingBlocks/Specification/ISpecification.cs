using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace order_system_modular_monolith.BuildingBlocks.Specification
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Criteria { get; }
    }
}
