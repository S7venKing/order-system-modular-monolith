using MediatR;

namespace order_system_modular_monolith.BuildingBlocks.CQRS;

public interface ICommand : ICommand<Unit>
{
}

public interface ICommand<out T> : IRequest<T>
    where T : notnull
{
}