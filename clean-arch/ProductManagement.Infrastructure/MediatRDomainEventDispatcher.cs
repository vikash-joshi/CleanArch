using MediatR;
using ProductManagement.Application.Events;
using ProductManagement.Domain.Common;
namespace ProductManagement.Infrastructure;

public class MediatRDomainEventDispatcher
{
    private readonly IPublisher _publisher;
    public MediatRDomainEventDispatcher(IPublisher publisher) => _publisher = publisher;

    public async Task DispatchAndClearEvents(IEnumerable<Entity> entities, CancellationToken ct)
    {
        foreach (var entity in entities)
        {
            var events = entity.GetDomainEvents().ToList();
            entity.ClearDomainEvents();

            foreach (var domainEvent in events)
            {
                var notificationType = typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType());
                var notification = Activator.CreateInstance(notificationType, domainEvent);
                await _publisher.Publish(notification!, ct);
            }
        }
    }
}