namespace ProductManagement.Domain.Entities;

public class OrderItem
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    internal OrderItem(Guid productId, int quantity, decimal unitPrice)
    {
        Id = Guid.NewGuid();
        ProductId = productId;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }
    
}
