

using NSubstitute;
using ProductManagement.Application.Interfaces;
using ProductManagement.Application.Commands;
using Microsoft.Extensions.Logging;

public class DeleteCommandHandlerTest
{
    [Fact]
    public async Task Delete_Soft_SetFlag_Deleted()
    {

        // var uow  = Substitute.For<IUnitOfWork>();
        
        // var product = new Product(Guid.NewGuid(),"Vkicy","abc",new Money(10,"INR"),1);
        // uow.Products.GetByIdAsync(Guid.NewGuid(),default).Returns(product);

        // var result = new  DeleteProductCommandHandler(uow);

        // await result.Handle(new DeleteProductCommand(product.Id),default);

  var uow  = Substitute.For<IUnitOfWork>();
        var logger = Substitute.For<ILogger<DeleteProductCommandHandler>>();
        var product = new Product(Guid.NewGuid(),"Vkicy","abc",new Money(10,"INR"),1);
        uow.Products.GetByIdAsync(product.Id,default).Returns(product);

        var result = new  DeleteProductCommandHandler(uow,logger);

        await result.Handle(new DeleteProductCommand(product.Id,default),default);

        Assert.True(product.IsDeleted);
        // Given
    
        // When
    
        // Then
    }
}