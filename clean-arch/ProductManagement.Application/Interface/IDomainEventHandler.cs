using ProductManagement.Domain.Common;

namespace ProductManagement.Application.Interfaces;

public interface IDomainEventHandler<in TEvent> where TEvent : DomainEvent
{
    Task Handle(TEvent domainEvent, CancellationToken cancellationToken);
}
