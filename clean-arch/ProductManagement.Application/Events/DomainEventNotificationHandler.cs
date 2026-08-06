using ProductManagement.Application.Events; 
using MediatR;
using ProductManagement.Domain.Common;
using ProductManagement.Application.Interfaces;
using ProductManagement.Application.EventHanlders;

namespace ProductManagement.Application.Events;

public class DomainEventNotificationHandler<TEvent> : MediatR.INotificationHandler<DomainEventNotification<TEvent>> where TEvent : DomainEvent
{
    private readonly IDomainEventHandler<TEvent> _handler;
    public DomainEventNotificationHandler(IDomainEventHandler<TEvent> handler) => _handler = handler;

    public Task Handle(DomainEventNotification<TEvent> notification, CancellationToken ct)
        => _handler.Handle(notification.DomainEvent, ct);

}
