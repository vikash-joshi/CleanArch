using Xunit;
using NSubstitute;
using ProductManagement.Application.Interfaces;

public class GetProductByIdQueryHandlerTests
{
    [Fact]
    public async Task Handle_ProductExists_ReturnsProductDto()
    {
        // STEP A — create a fake IUnitOfWork (no real database)
        var uow = Substitute.For<IUnitOfWork>();

        // STEP B — create a real Product object to pretend the DB "has"
        var product = new Product(Guid.NewGuid(), "Keyboard", "desc", new Money(999, "INR"), 10);

        // STEP C — tell the fake: "when GetByIdAsync is called with ANY guid, return this product"
        uow.Products.GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(product);

        // STEP D — create the REAL handler, but hand it the FAKE uow
        var handler = new GetProductByIdQueryHandler(uow);

        // STEP E — actually call the method you're testing
        var result = await handler.Handle(new GetProductByIdQuery(product.Id), default);

        // STEP F — check the result is what you expected
        Assert.NotNull(result);
        Assert.Equal("Keyboard", result!.Name);
    }

    [Fact]
    public async Task Handle_ProductDoesNotExist_ReturnsNull()
    {
        // Arrange
        var uow = Substitute.For<IUnitOfWork>();

        uow.Products
            .GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns((Product?)null);

        var handler = new GetProductByIdQueryHandler(uow);

        // Act
        var result = await handler.Handle(
            new GetProductByIdQuery(Guid.NewGuid()),
            default);

        // Assert
        Assert.Null(result);
    }
}