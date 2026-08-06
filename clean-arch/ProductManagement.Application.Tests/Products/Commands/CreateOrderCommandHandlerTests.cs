using Microsoft.Extensions.Logging;
using NSubstitute;
using ProductManagement.Application.BusinessLogics;
using ProductManagement.Application.Commands;
using ProductManagement.Application.Interfaces;
using ProductManagement.Application.Strategies;
public class CreateOrderCommandHandlerTest
{
    [Fact]
    public void  Handle_No_Discount()
    {   
        // Arrange
       var product = new Product(Guid.NewGuid(), "Item", "desc", new Money(100, "INR"), 50);
       var result = new StandardPricingStrategy().CalculatePrice(product, 15);

    Assert.Equal(1500, result); // (100*15) * 0.9

    }

    [Fact]
    public void Hanlde_BulkDiscount()
    {
        var product = new Product(Guid.NewGuid(), "Item", "desc", new Money(100, "INR"), 1);
        var result = new BulkDiscountPricingStrategy().CalculatePrice(product, 11);

        Assert.Equal(990,result);

    }

    [Fact]
    public void Handle_SeasonalDiscount()
    {
        var product = new Product(Guid.NewGuid(), "Item", "desc", new Money(100, "INR"), 1);
        var result = new SeasonalPricingStrategy().CalculatePrice(product, 15);

        Assert.Equal(1200,result);
    }

}