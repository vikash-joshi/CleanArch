using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ProductManagement.Application.Events;
using ProductManagement.Application.Interfaces;
using ProductManagement.Domain.Common;
namespace ProductManagement.Infrastructure;

public class MediatRDomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IServiceProvider _serviceProvider;

    public MediatRDomainEventDispatcher(IServiceProvider serviceProvider)
        => _serviceProvider = serviceProvider;

    public async Task DispatchAndClearEvents(IEnumerable<Entity> entitiesWithEvents, CancellationToken ct)
    {
        foreach (var entity in entitiesWithEvents)
        {
            var events = entity.GetDomainEvents().ToList();
            entity.ClearDomainEvents();

            foreach (var domainEvent in events)
            {
                var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(domainEvent.GetType());
                var handlers = _serviceProvider.GetServices(handlerType);

                foreach (var handler in handlers)
                {
                    var method = handlerType.GetMethod("Handle");
                    await (Task)method!.Invoke(handler, new object[] { domainEvent, ct })!;
                }
            }
        }
    }
}