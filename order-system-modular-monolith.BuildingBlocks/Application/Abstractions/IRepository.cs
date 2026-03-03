using order_system_modular_monolith.BuildingBlocks.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace order_system_modular_monolith.BuildingBlocks.Application.Abstractions
{
    public interface IRepository<T, TId>
        where T : AggregateRoot<TId>
    {
        Task AddAsync(T entity);
        Task Update(T entity);
        Task Remove(T entity);
    }
}
