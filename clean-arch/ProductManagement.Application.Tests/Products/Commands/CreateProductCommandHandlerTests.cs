using NSubstitute;
using ProductManagement.Application.Interfaces;
using ProductManagement.Application.Commands;
using ProductManagement.Application.BusinessLogics;
using Microsoft.Extensions.Logging;
public class CreateProductCommandHandlerTests
{
  [Fact]
  public async Task Handle_ProductDoesNotExist_ReturnsNull()
  {
    // Arrange
    var uow = Substitute.For<IUnitOfWork>();
    var rules = new ProductBusinessRules(Substitute.For<IProductRepository>());
    var logger = Substitute.For<ILogger<CreateProductCommandHandler>>();


    var handler = new CreateProductCommandHandler(uow, rules, logger);

    var result = await handler.Handle(new CreateProductCommand("", "Vikash", 99, 10), default);

    Assert.False(result.IsSuccess);
    Assert.Equal("Name is required.", result.Error);


  }

  [Fact]
  public async Task Handle_Product_Success()
  {
    // Arrange
    var uow = Substitute.For<IUnitOfWork>();
    
    var rules = new ProductBusinessRules(Substitute.For<IProductRepository>());
    var logger = Substitute.For<ILogger<CreateProductCommandHandler>>();

    var handler = new CreateProductCommandHandler(uow, rules, logger);

    var result = await handler.Handle(new CreateProductCommand("Vicky", "Vikash", 99, 10), default);

    Assert.True(result.IsSuccess);


  }
}