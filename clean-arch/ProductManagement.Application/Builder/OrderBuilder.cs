namespace ProductManagement.Application.Builder;

// Lightweight Order type to satisfy builder output. Move to its own file if shared elsewhere.
public record Order(Guid Id, Guid ProductId, int Quantity, string ShippingAddress, decimal Price);

public class OrderBuilder
{
    private Guid? _productId;
    private string? _shippingAddress;
    private Func<Guid, int, decimal>? _pricingStrategy;
    private int _quantity = 1;

    public OrderBuilder WithProduct(Guid product)
    {
        _productId = product;
        return this;
    }

    public OrderBuilder WithQuantity(int quantity)
    {
        _quantity = quantity;
        return this;
    }

    public OrderBuilder WithShippingAddress(string address)
    {
        _shippingAddress = address;
        return this;
    }

    public OrderBuilder WithPricingStrategy(Func<Guid, int, decimal> strategy)
    {
        _pricingStrategy = strategy;
        return this;
    }

    public Order Build()
    {
        if (_productId is null)
            throw new InvalidOperationException("Product is required to build an Order.");
        if (string.IsNullOrWhiteSpace(_shippingAddress))
            throw new InvalidOperationException("Shipping address is required to build an Order.");
        if (_pricingStrategy is null)
            throw new InvalidOperationException("Pricing strategy is required to build an Order.");

        var finalPrice = _pricingStrategy(_productId.Value, _quantity);
        return new Order(Guid.NewGuid(), _productId.Value, _quantity, _shippingAddress, finalPrice);
    }
}
