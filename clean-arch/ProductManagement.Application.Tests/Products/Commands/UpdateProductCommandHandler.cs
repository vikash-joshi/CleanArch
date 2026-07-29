using NSubstitute;
using ProductManagement.Application.Interfaces;
using ProductManagement.Application.Commands;
using Microsoft.Extensions.Logging;
using Xunit;

public class UpdateProductCommandHandlerTest
{
    [Fact]
    public async Task UpdateProduct_Handle_Product_DoesNotExist()
    {
        var uow = Substitute.For<IUnitOfWork>();
        var logger = Substitute.For<ILogger<UpdateProductCommandHandler>>();
        // Given

        uow.Products.GetByIdAsync(Arg.Any<Guid>(),Arg.Any<CancellationToken>()).Returns((Product?)null);

        var handler = new UpdateProductCommandHandler(uow,logger);
        var product = await handler.Handle(
        new UpdateProductCommand(Guid.NewGuid().ToString(),"Fake","Fake",0,1),default);
        Assert.False(product.IsSuccess);
        Assert.Equal("Product not found.", product.Error);


    
        // When
    
        // Then
    }


        [Fact]
    public async Task UpdateProduct_Handle_Product_DoesExist_UpdateSuccess()
    {
        var uow = Substitute.For<IUnitOfWork>();
                var logger = Substitute.For<ILogger<UpdateProductCommandHandler>>();

        // Given
        var guid = Guid.NewGuid();
        uow.Products.GetByIdAsync(guid,Arg.Any<CancellationToken>()).Returns(new Product(guid, "abc", "desc", new Money(999, "INR"), 10));

        var handler = new UpdateProductCommandHandler(uow,logger);
        var product = await handler.Handle(
        new UpdateProductCommand(guid.ToString(),"abcc","Fake",0,1),default);
        Assert.True(product.IsSuccess);


    
        // When
    
        // Then
    }
}