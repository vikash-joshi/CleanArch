using Xunit;
using NSubstitute;
using ProductManagement.Application.Commands;
using ProductManagement.Application.Interfaces;

public class CreateProductCommandHandlerTests
{
    [Fact]
    public async Task Handle_EmptyName_ReturnsFailure()
    {
        // Arrange — set up a fake IUnitOfWork so no real DB is needed
        var uow = Substitute.For<IUnitOfWork>();
        var handler = new CreateProductCommandHandler(uow);

        // Act — call the method you're testing
        var result = await handler.Handle(
            new CreateProductCommand("", "desc", 100, 5), default);

        // Assert — check it did what you expect
        Assert.False(result.IsSuccess);
        Assert.Equal("Name is required.", result.Error);
    }
}