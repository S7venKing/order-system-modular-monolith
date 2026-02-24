using System;
using System.Collections.Generic;
using System.Text;

namespace order_system_modular_monolith.BuildingBlocks.Application.Abstractions
{
    public interface IReadRepository<T, TId>
    {
        Task<T?> GetByIdAsync(TId id);
    }
}
