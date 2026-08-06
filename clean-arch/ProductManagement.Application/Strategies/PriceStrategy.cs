namespace ProductManagement.Application.Strategies;

public class StandardPricingStrategy : IPriceStrategy
{
    public decimal CalculatePrice(Product product, int quantity)
    {
        return product.Price.Amount * quantity;
    }
}

public class BulkDiscountPricingStrategy : IPriceStrategy
{
    public decimal CalculatePrice(Product product, int quantity)
    {
         var total = product.Price.Amount * quantity;
        return quantity > 10 ? total * 0.9m : total; // 10% off
    }
}



public class SeasonalPricingStrategy : IPriceStrategy
{
   private readonly int[] _saleMonths = { 8, 12 }; // e.g. Nov/Dec sale

    public decimal CalculatePrice(Product product, int quantity)
    {
        var total = product.Price.Amount * quantity;
        
        return _saleMonths.Contains(DateTime.UtcNow.Month) ? total * 0.8m : total; // 20% off
    }
}

