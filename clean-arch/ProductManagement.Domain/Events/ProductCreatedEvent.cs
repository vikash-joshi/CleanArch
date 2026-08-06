namespace ProductManagement.Domain.Common;
public record ProductCreatedEvent(Guid ProductId, string Name) : DomainEvent;