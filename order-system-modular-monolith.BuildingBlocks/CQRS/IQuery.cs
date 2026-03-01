using MediatR;

namespace order_system_modular_monolith.BuildingBlocks.CQRS;

public interface IQuery<out T> : IRequest<T>
    where T : notnull
{
}