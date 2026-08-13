using ProductManagement.Domain.Common;

namespace ProductManagement.Domain.Events;

public record OrderPlacedEvent(Guid OrderId) : DomainEvent;
