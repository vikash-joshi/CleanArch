
using MediatR;
using Microsoft.Extensions.Logging;
using ProductManagement.Application.BusinessLogics;
using ProductManagement.Application.Interfaces;
namespace ProductManagement.Application.Commands;

public class AssignCategoryToProductHandler : IRequestHandler<AssignCategoryToProductCommand, Result<bool>>
{
    private readonly IUnitOfWork _uow;

    public AssignCategoryToProductHandler(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<Result<bool>> Handle(AssignCategoryToProductCommand request, CancellationToken cancellationToken)
    {
        var productExists = await _uow.Products.GetByIdAsync(request.ProductId, cancellationToken);
        if (productExists is null)
        {
            return Result<bool>.Failure("Product not found.");
        }

        var categoryExists = await _uow.Categories.GetCategoryByIdAsync(request.CategoryId, cancellationToken);
        if (categoryExists is null)
        {
            return Result<bool>.Failure("Category not found.");
        }

        await _uow.Products.AssignCategoryToProductAsync(request.ProductId, request.CategoryId, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        return Result<bool>.Success(true);
    }
}