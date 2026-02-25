using order_system_modular_monolith.BuildingBlocks.Domain;
using System.Linq.Expressions;

namespace order_system_modular_monolith.BuildingBlocks.Mongo;

public interface IReadRepository<TEntity, in TId>
    where TEntity : AggregateRoot<TId>
        where TId : StronglyTypedId
{
    Task<TEntity?> FindByIdAsync(TId id, CancellationToken cancellationToken = default);

    Task<TEntity?> FindOneAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<TEntity>> FindAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

    public Task<IReadOnlyList<TEntity>> RawQuery(
        string query,
        CancellationToken cancellationToken = default,
        params object[] queryParams);
}

public interface IWriteRepository<TEntity, in TId>
    where TEntity : AggregateRoot<TId>
        where TId : StronglyTypedId
{
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task DeleteRangeAsync(IReadOnlyList<TEntity> entities, CancellationToken cancellationToken = default);
    Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
    Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task DeleteByIdAsync(TId id, CancellationToken cancellationToken = default);
}

public interface IRepository<TEntity, in TId> :
    IReadRepository<TEntity, TId>,
    IWriteRepository<TEntity, TId>,
    IDisposable
    where TEntity : AggregateRoot<TId>
        where TId : StronglyTypedId
{
}
