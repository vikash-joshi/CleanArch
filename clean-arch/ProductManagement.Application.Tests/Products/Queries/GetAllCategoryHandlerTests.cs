using Xunit;
using NSubstitute;
using ProductManagement.Application.Interfaces;
using Castle.Core.Logging;

public class GetCategoryHandlerTests
{
    [Fact]
    public async Task Handle_All_Category_ReturnsCategoryDto()
    {
        // STEP A — create a fake IUnitOfWork (no real database)
        var uow = Substitute.For<IUnitOfWork>();
        //var logger = Substitute.For<ILogger<GetAllCategoriesQueryHandler>>();
        // STEP B — create a real Category object to pretend the DB "has"
        var category = new Category(Guid.NewGuid(), "Electronics", "Devices and gadgets");

        // STEP C — tell the fake: "when GetAllCategoriesQuery is called, return this category"
        uow.Categories.GetAllCategoryAsync(Arg.Any<CancellationToken>())
            .Returns(new List<Category> { category });

        // STEP D — create the REAL handler, but hand it the FAKE uow
        var handler = new GetAllCategoriesQueryHandler(uow);

        // STEP E — actually call the method you're testing
        var result = await handler.Handle(new GetAllCategoriesQuery(1, 10, ""), default);

        // STEP F — check the result is what you expected
        Assert.NotNull(result);


    }
}