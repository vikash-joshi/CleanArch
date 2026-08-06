using MediatR;
using ProductManagement.Domain.Common;
using ProductManagement.Application.Interfaces;
using ProductManagement.Application.EventHanlders;

namespace ProductManagement.Application.Events;

public class DomainEventNotification<TEvent> : INotification where TEvent : DomainEvent
{
    public TEvent DomainEvent { get; }
    public DomainEventNotification(TEvent domainEvent) => DomainEvent = domainEvent;

}
