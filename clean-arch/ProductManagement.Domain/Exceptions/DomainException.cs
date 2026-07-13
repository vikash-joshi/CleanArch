public abstract class DomainException : Exception
{
    protected DomainException(string message) : base(message) { }
}

public class InsufficientStockException : DomainException
{
    public InsufficientStockException(Guid productId, int requested, int available)
        : base($"Product {productId} has {available} in stock, requested {requested}.") { }
}