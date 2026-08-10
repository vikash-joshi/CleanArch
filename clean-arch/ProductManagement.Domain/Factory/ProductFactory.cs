namespace ProductManagement.Domain.Factories;

public class ProductFactory
{
  public static Product Create(string name, string description, decimal price, int stockQuantity, Guid? categoryId = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.");
        if (price < 0)
            throw new ArgumentException("Price cannot be negative.");
        if (stockQuantity < 0)
            throw new ArgumentException("Stock cannot be negative.");

        var product = new Product(Guid.NewGuid(), name, description, new Money(price, "INR"), stockQuantity);

        if (categoryId.HasValue)
            product.AssignCategory(categoryId.Value);

        // domain event (ProductCreatedEvent) is already raised inside Product's constructor from Day 16 — no need to duplicate it here
        return product;
    }

}
