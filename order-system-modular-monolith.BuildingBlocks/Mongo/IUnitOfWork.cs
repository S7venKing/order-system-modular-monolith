namespace order_system_modular_monolith.BuildingBlocks.Mongo;

public interface IUnitOfWork : IDisposable
{
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitAsync(CancellationToken cancellationToken = default);
}

public interface IUnitOfWork<out TContext> : IUnitOfWork
    where TContext : class
{
    TContext Context { get; }
}