namespace ProductManagement.Application.Strategies;

public class PricingStrategyFactory
{
    public IPriceStrategy GetStrategy(int quantity)
    {
        if (DateTime.UtcNow.Month is 11 or 12)
            return new SeasonalPricingStrategy();

        if (quantity > 10)
            return new BulkDiscountPricingStrategy();

        return new StandardPricingStrategy();
    }
}
