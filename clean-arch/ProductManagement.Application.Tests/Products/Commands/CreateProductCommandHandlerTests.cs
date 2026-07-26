using NSubstitute;
using ProductManagement.Application.Interfaces;
using ProductManagement.Application.Commands;

public class CreateProductCommandHandlerTests
{
[Fact]
    public async Task Handle_ProductDoesNotExist_ReturnsNull()
    {
        // Arrange
        var uow = Substitute.For<IUnitOfWork>();

        var handler = new CreateProductCommandHandler(uow);

        var result = await handler.Handle(new CreateProductCommand( "","Vikash",99,10),default);

      Assert.False(result.IsSuccess);
        Assert.Equal("Name is required.", result.Error);

        
    }

    [Fact]
    public async Task Handle_Product_Success()
    {
        // Arrange
        var uow = Substitute.For<IUnitOfWork>();

        var handler = new CreateProductCommandHandler(uow);

        var result = await handler.Handle(new CreateProductCommand("Vicky","Vikash",99,10),default);

      Assert.True(result.IsSuccess);

        
    }
}