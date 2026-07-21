using MediatR;
using ProductManagement.Application.Commands;
using ProductManagement.Application.Interfaces;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Result<bool>>
{
    public readonly IUnitOfWork _unitOfWork;

    public DeleteProductCommandHandler(IUnitOfWork _unitOfWork) => this._unitOfWork = _unitOfWork;

    public async Task<Result<bool>> Handle(DeleteProductCommand command,CancellationToken token)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(command.Id, token);
        if (product == null)
        {
            return Result<bool>.Failure("Product not found.");
        }
        product.Delete();
        await _unitOfWork.Products.UpdateAsync(product, token);
        await _unitOfWork.SaveChangesAsync(token);

        return Result<bool>.Success(true);
    }
}