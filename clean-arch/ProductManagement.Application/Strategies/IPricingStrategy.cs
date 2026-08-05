namespace ProductManagement.Application.Strategies;

public interface IPriceStrategy
{
    decimal CalculatePrice(Product product, int quantity);

}