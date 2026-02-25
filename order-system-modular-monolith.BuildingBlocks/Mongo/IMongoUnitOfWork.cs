namespace order_system_modular_monolith.BuildingBlocks.Mongo;

public interface IMongoUnitOfWork<out TContext> : IUnitOfWork<TContext> where TContext : class, IMongoDbContext
{
}