using MediatR;
using Microsoft.Extensions.Logging;
using ProductManagement.Application.Commands;
using ProductManagement.Application.Interfaces;

public class UpdateCategoryHandler:IRequestHandler<UpdateCategoryCommand, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCategoryHandler(IUnitOfWork _unitOfWork)
    {
        this._unitOfWork = _unitOfWork;
    }

    public async Task<Result<bool>> Handle(UpdateCategoryCommand Req,CancellationToken ct)
    {
        var ExistingCategory = await _unitOfWork.Categories.GetCategoryByIdAsync(Req.Id, ct);

        if(ExistingCategory == null)
        {
            return Result<bool>.Failure("Category not found.");
        }   

        ExistingCategory.UpdateCategory(Req.Name, Req.Description);
        await _unitOfWork.Categories.UpdateCategoryAsync(ExistingCategory, ct);
        await _unitOfWork.SaveChangesAsync(ct);
        return Result<bool>.Success(true);
    }
}