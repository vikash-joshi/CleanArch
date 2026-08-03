using NSubstitute;
using ProductManagement.Application.Interfaces;
using ProductManagement.Application.Commands;
using Microsoft.Extensions.Logging;
using Xunit;

public class UpdateCategoryCommandHandlerTest
{
    [Fact]
    public async Task UpdateCategory_Handle_Category_DoesNotExist()
    {
        var uow = Substitute.For<IUnitOfWork>();
        var logger = Substitute.For<ILogger<UpdateCategoryHandler>>();
        // Given

        uow.Categories.GetCategoryByIdAsync(Arg.Any<Guid>(),Arg.Any<CancellationToken>()).Returns((Category?)null);

        var handler = new UpdateCategoryHandler(uow);
        var category = await handler.Handle(
        new UpdateCategoryCommand(Guid.NewGuid(),"Fake","Fake"),default);
        Assert.False(category.IsSuccess);
        Assert.Equal("Category not found.", category.Error);


    
        // When
    
        // Then
    }


        //[Fact]
    // public async Task UpdateProduct_Handle_Product_DoesExist_UpdateSuccess()
    // {
    //     var uow = Substitute.For<IUnitOfWork>();
    //             var logger = Substitute.For<ILogger<UpdateProductCommandHandler>>();

    //     // Given
    //     var guid = Guid.NewGuid();
    //     uow.Products.GetByIdAsync(guid,Arg.Any<CancellationToken>()).Returns(new Product(guid, "abc", "desc", new Money(999, "INR"), 10));

    //     var handler = new UpdateProductCommandHandler(uow,logger);
    //     var product = await handler.Handle(
    //     new UpdateProductCommand(guid.ToString(),"abcc","Fake",0,1),default);
    //     Assert.True(product.IsSuccess);


    
    //     // When
    
    //     // Then
    // }
}