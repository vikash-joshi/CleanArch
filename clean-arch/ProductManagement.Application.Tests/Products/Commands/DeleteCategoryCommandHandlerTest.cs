

using NSubstitute;
using ProductManagement.Application.Interfaces;
using ProductManagement.Application.Commands;
using Microsoft.Extensions.Logging;

public class DeleteCategoryCommandHandlerTest
{
    [Fact]
    public async Task Delete_Soft_SetFlag_Deleted()
    {
        var uow  = Substitute.For<IUnitOfWork>();
        var logger = Substitute.For<ILogger<DeleteCategoryCommandHandler>>();
        var category = new Category(Guid.NewGuid(),"Vicky","Vikash");
        uow.Categories.GetCategoryByIdAsync(category.Id,default).Returns(category);

        var result = new  DeleteCategoryCommandHandler(uow,logger);

        await result.Handle(new DeleteCategoryCommand(category.Id,default),default);

        Assert.True(category.IsDeleted);
    }
}