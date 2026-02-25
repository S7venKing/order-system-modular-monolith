using order_system_modular_monolith.BuildingBlocks.Application.Abstractions;
using order_system_modular_monolith.BuildingBlocks.Domain;

namespace order_system_modular_monolith.BuildingBlocks.Mongo;

public interface IMongoRepository<TEntity, in TId> : IRepository<TEntity, TId>
    where TEntity : AggregateRoot<TId>
    where TId : StronglyTypedId
{
}