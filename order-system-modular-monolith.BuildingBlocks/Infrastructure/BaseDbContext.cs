using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using order_system_modular_monolith.BuildingBlocks.Domain;

namespace order_system_modular_monolith.BuildingBlocks.Infrastructure
{
    public abstract class BaseDbContext : DbContext
    {
        private readonly IDomainEventDispatcher _dispatcher;

        protected BaseDbContext(
            DbContextOptions options,
            IDomainEventDispatcher dispatcher)
            : base(options)
        {
            _dispatcher = dispatcher;
        }

        public override async Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default)
        {
            var domainEvents = ChangeTracker
                .Entries<IHasDomainEvents>()
                .Select(e => e.Entity)
                .SelectMany(e =>
                {
                    var events = e.DomainEvents;
                    e.ClearDomainEvents();
                    return events;
                })
                .ToList();

            var result = await base.SaveChangesAsync(cancellationToken);

            await _dispatcher.DispatchAsync(domainEvents);

            return result;
        }
    }
}
