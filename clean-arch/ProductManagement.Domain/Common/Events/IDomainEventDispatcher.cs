using ProductManagement.Domain.Common;

public interface IDomainEventDispatcher
{
    Task DispatchAndClearEvents(IEnumerable<Entity> entitiesWithEvents, CancellationToken ct);
}
