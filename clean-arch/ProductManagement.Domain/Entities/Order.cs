

using ProductManagement.Domain.Common;
using ProductManagement.Domain.Enums;
using ProductManagement.Domain.Events;
namespace ProductManagement.Domain.Entities;

public class Order :Entity
{
    private readonly List<OrderItem> _items = new();
    public Guid Id { get; set; }    
    public string ShippingAddress { get; set; }
    public OrderStatus Status { get; set; }
    public IReadOnlyCollection<OrderItem> OrderItems => _items.AsReadOnly();

    public Order(Guid id, string shippingAddress)
    {
        if (string.IsNullOrWhiteSpace(shippingAddress))
            throw new ArgumentException("Shipping address is required.");

        Id = id;
        ShippingAddress = shippingAddress;
        Status = OrderStatus.Pending;
    }

    public void AddItem(Guid productId, int quantity, decimal unitPrice, int stockQuantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.");
        if (quantity > stockQuantity)
            throw new ArgumentException("Insufficient stock available.");

        if (unitPrice <= 0)
            throw new ArgumentException("Unit price must be greater than zero.");

        _items.Add(new OrderItem(productId, quantity, unitPrice));
    }
    
    public void MarkPlaced()
    {
        AddDomainEvent(new OrderPlacedEvent(Id));
    }
}
