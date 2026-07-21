using MediatR;
using ProductManagement.Application.Commands;
using ProductManagement.Application.Interfaces;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result<bool>>
{
    public readonly IUnitOfWork _unitOfWork;

    public UpdateProductCommandHandler(IUnitOfWork _unitOfWork) => this._unitOfWork = _unitOfWork;

    public async Task<Result<bool>> Handle(UpdateProductCommand command,CancellationToken token)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(new Guid(command.id), token);
        if (product == null)
        {
            return Result<bool>.Failure("Product not found.");
        }

          if (string.IsNullOrEmpty(product?.Name))
        {
            return Result<bool>.Failure("Product Name Is Blank.");
        }

        product.UpdateDetails(command.Name, command.Description, new Money(command.Price,"USD"),command.Stock);

        await _unitOfWork.Products.UpdateAsync(product, token);
        await _unitOfWork.SaveChangesAsync(token);

        return Result<bool>.Success(true);
    }
}