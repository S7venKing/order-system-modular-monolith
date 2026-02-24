using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace order_system_modular_monolith.BuildingBlocks.Specification
{
    public abstract class Specification<T>
        : ISpecification<T>
    {
        public abstract Expression<Func<T, bool>> Criteria { get; }
    }
}
