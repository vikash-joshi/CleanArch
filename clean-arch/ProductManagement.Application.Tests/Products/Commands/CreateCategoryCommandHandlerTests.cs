using NSubstitute;
using ProductManagement.Application.Interfaces;
using ProductManagement.Application.Commands;
using ProductManagement.Application.BusinessLogics;
using Microsoft.Extensions.Logging;
public class CreateCategoryCommandHandlerTests
{
[Fact]
    public async Task Handle_CategoryDoesNotExist_ReturnsNull()
    {
        // Arrange
        var uow = Substitute.For<IUnitOfWork>();
        var rules = Substitute.For<CategoryBusinessRules>();
        var logger = Substitute.For<ILogger<CreateCategoryCommandHandler>>();


        var handler = new CreateCategoryCommandHandler(uow,rules,logger);

        var result = await handler.Handle(new CreateCategoryCommand( "","Vikash"),default);

        Assert.False(result.IsSuccess);
        Assert.Equal("Name is required.", result.Error);

        
    }

    [Fact]
    public async Task Handle_Category_Success()
    {
        // Arrange
        var uow = Substitute.For<IUnitOfWork>();
                var rules = Substitute.For<CategoryBusinessRules>();
                        var logger = Substitute.For<ILogger<CreateCategoryCommandHandler>>();

        var handler = new CreateCategoryCommandHandler(uow,rules,logger);

        var result = await handler.Handle(new CreateCategoryCommand("Vicky","Vikash"),default);

      Assert.True(result.IsSuccess);

        
    }
}