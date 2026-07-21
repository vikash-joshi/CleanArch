

public class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Money Price { get; private set; }
    public int StockQuantity { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTime? DeletedAt { get; private set; }


    public Product(Guid id, string name, string description, Money price, int stockQuantity)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required");
        if (price is null) throw new ArgumentException("Price is required");

        Id = id;
        Name = name;
        Description = description;
        Price = price;
        StockQuantity = stockQuantity;
    }

    public void DecreaseStock(int qty)
    {
        if (qty > StockQuantity)
            throw new InsufficientStockException(Id, qty, StockQuantity);
        StockQuantity -= qty;
    }

    public void Delete()
    {
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
    }

    public void UpdateDetails(string name, string description, Money price, int stockQuantity)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required");
        Name = name;
        Description = description;
        Price = price;
        StockQuantity = stockQuantity;
    }
}
