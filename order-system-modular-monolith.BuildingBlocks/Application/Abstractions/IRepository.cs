using order_system_modular_monolith.BuildingBlocks.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace order_system_modular_monolith.BuildingBlocks.Application.Abstractions
{
    public interface IRepository<T, TId>
        where T : AggregateRoot<TId>
        where TId : StronglyTypedId
    {
        Task AddAsync(T entity);
        void Update(T entity);
        void Remove(T entity);
    }
}
