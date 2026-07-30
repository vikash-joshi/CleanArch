
using MediatR;
using Microsoft.Extensions.Logging;
using ProductManagement.Application.Interfaces;

public class DeleteCategoryCommandHandler:IRequestHandler<DeleteCategoryCommand,Result<bool>>
{
    private readonly IUnitOfWork _uow;
    private readonly ILogger<DeleteCategoryCommandHandler> _logger;
    public DeleteCategoryCommandHandler(IUnitOfWork uow, ILogger<DeleteCategoryCommandHandler> logger)
    {
        _uow = uow;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(DeleteCategoryCommand req,CancellationToken cts)
    {
        var category = await _uow.Categories.GetCategoryByIdAsync(req.Id, cts);
        if(category == null)
        {
            return Result<bool>.Failure("Category not found.");
        }

        category.MarkDeleted();
        await _uow.Categories.UpdateCategoryAsync(category, cts);
        await _uow.SaveChangesAsync(cts);
        return Result<bool>.Success(true);
    }
}