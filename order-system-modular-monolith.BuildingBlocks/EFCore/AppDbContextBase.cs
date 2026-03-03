using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using order_system_modular_monolith.BuildingBlocks.Application.Abstractions;
using order_system_modular_monolith.BuildingBlocks.Domain;
using order_system_modular_monolith.BuildingBlocks.EFCore;
using order_system_modular_monolith.BuildingBlocks.Web;
using System.Collections.Immutable;
using static order_system_modular_monolith.BuildingBlocks.Domain.Behaviors;
using IsolationLevel = System.Data.IsolationLevel;

namespace order_system_modular_monolith.BuildingBlocks.EFCore;

public abstract class AppDbContextBase<TContext>
    : DbContext, IDbContext
    where TContext : DbContext 
{
    private readonly ICurrentUserProvider? _currentUserProvider;
    private readonly ILogger<AppDbContextBase<TContext>>? _logger;
    private IDbContextTransaction _currentTransaction;
    private readonly IDateTimeProvider _dateTimeProvider;

    protected AppDbContextBase(
        DbContextOptions<TContext> options,
        ICurrentUserProvider? currentUserProvider = null,
        ILogger<AppDbContextBase<TContext>>? logger = null,
        IDateTimeProvider? dateTimeProvider = null)
        : base(options)
    {
        _currentUserProvider = currentUserProvider;
        _logger = logger;
        _dateTimeProvider = dateTimeProvider;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
    }

    public IExecutionStrategy CreateExecutionStrategy() => Database.CreateExecutionStrategy();

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction != null)
            return;

        _currentTransaction = await Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await SaveChangesAsync(cancellationToken);
            await _currentTransaction?.CommitAsync(cancellationToken)!;
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
        finally
        {
            _currentTransaction?.Dispose();
            _currentTransaction = null;
        }
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await _currentTransaction?.RollbackAsync(cancellationToken)!;
        }
        finally
        {
            _currentTransaction?.Dispose();
            _currentTransaction = null;
        }
    }


    //ref: https://learn.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency#execution-strategies-and-transactions
    public Task ExecuteTransactionalAsync(CancellationToken cancellationToken = default)
    {
        var strategy = CreateExecutionStrategy();
        return strategy.ExecuteAsync(async () =>
        {
            await using var transaction =
                await Database.BeginTransactionAsync(cancellationToken);
            try
            {
                await SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        });
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        OnBeforeSaving();
        try
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
        //ref: https://learn.microsoft.com/en-us/ef/core/saving/concurrency?tabs=data-annotations#resolving-concurrency-conflicts
        catch (DbUpdateConcurrencyException ex)
        {
            foreach (var entry in ex.Entries)
            {
                var databaseValues = await entry.GetDatabaseValuesAsync(cancellationToken);

                if (databaseValues == null)
                {
                    _logger.LogError("The record no longer exists in the database, The record has been deleted by another user.");
                    throw;
                }

                // Refresh the original values to bypass next concurrency check
                entry.OriginalValues.SetValues(databaseValues);
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }

    public IReadOnlyList<IDomainEvent> GetDomainEvents()
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

        return domainEvents.ToImmutableList();
    }

    // ref: https://www.meziantou.net/entity-framework-core-generate-tracking-columns.htm
    // ref: https://www.meziantou.net/entity-framework-core-soft-delete-using-query-filters.htm
    private void OnBeforeSaving()
    {
        var userId = _currentUserProvider?.GetCurrentUserId() ?? 0;
        var now = _dateTimeProvider.UtcNow;

        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is IAuditable auditable)
            {
                if (entry.State == EntityState.Added)
                {
                    auditable.CreatedBy = userId;
                    auditable.CreatedAt = now;
                }

                if (entry.State == EntityState.Modified)
                {
                    auditable.LastModifiedBy = userId;
                    auditable.LastModified = now;
                }
            }

            if (entry.Entity is IVersioned versioned &&
                entry.State == EntityState.Modified)
            {
                versioned.Version++;
            }

            if (entry.Entity is ISoftDelete softDelete &&
                entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                softDelete.IsDeleted = true;

                if (entry.Entity is IAuditable auditableSoft)
                {
                    auditableSoft.LastModifiedBy = userId;
                    auditableSoft.LastModified = now;
                }

                if (entry.Entity is IVersioned versionedSoft)
                {
                    versionedSoft.Version++;
                }
            }
        }
    }
}