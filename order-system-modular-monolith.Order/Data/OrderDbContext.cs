using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using order_system_modular_monolith.BuildingBlocks.Application.Abstractions;
using order_system_modular_monolith.BuildingBlocks.Domain;
using order_system_modular_monolith.BuildingBlocks.EFCore;
using order_system_modular_monolith.BuildingBlocks.Infrastructure;
using order_system_modular_monolith.BuildingBlocks.Web;

namespace order_system_modular_monolith.Order.Data
{
    public class OrderDbContext : AppDbContextBase<OrderDbContext>
    {
        private readonly ICurrentUserProvider? _currentUserProvider;
        private readonly ILogger<AppDbContextBase<OrderDbContext>>? _logger;
        private IDbContextTransaction _currentTransaction;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IDomainEventDispatcher _domainEventDispatcher;
        public const string Schema = "orders";

        public OrderDbContext(DbContextOptions<OrderDbContext> options, ICurrentUserProvider? currentUserProvider = null, ILogger<AppDbContextBase<OrderDbContext>>? logger = null, IDateTimeProvider? dateTimeProvider = null, IDomainEventDispatcher? domainEvent = null) : base(options, currentUserProvider, logger, dateTimeProvider, domainEvent)
        {
            _currentUserProvider = currentUserProvider;
            _logger = logger;
            _dateTimeProvider = dateTimeProvider;
            _domainEventDispatcher = domainEvent!;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schema);
            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(OrderDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
