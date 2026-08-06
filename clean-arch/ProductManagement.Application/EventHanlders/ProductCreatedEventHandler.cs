using Microsoft.Extensions.Logging;
using ProductManagement.Application.Interfaces;
using ProductManagement.Domain.Common;

namespace ProductManagement.Application.EventHanlders;

public class ProductCreatedEventHandler : INotificationHandler<ProductCreatedEvent>
{
    private readonly ILogger<ProductCreatedEventHandler> _logger;

    public ProductCreatedEventHandler(ILogger<ProductCreatedEventHandler> logger)
    {
        _logger = logger;
    }

     public Task Handle(ProductCreatedEvent domainEvent, CancellationToken ct)
    {
        _logger.LogInformation("Domain Event: Product {ProductId} - {Name} was created at {OccurredOn}",
            domainEvent.ProductId, domainEvent.Name, domainEvent.OccurredOn);
        return Task.CompletedTask;
    }

}
