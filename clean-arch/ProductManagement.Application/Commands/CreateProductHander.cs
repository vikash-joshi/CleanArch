using MediatR;
using ProductManagement.Application.Interfaces;
namespace ProductManagement.Application.Commands;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<Guid>>
{
    private readonly IUnitOfWork _uow;

    public CreateProductCommandHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<Result<Guid>> Handle(CreateProductCommand command, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(command.Name))
            return Result<Guid>.Failure("Name is required.");

        if (command.Price < 0)
            return Result<Guid>.Failure("Price cannot be negative.");

        var product = new Product(
            Guid.NewGuid(),
            command.Name,
            command.Description,
            new Money(command.Price, "INR"),
            command.Stock);

        await _uow.Products.AddAsync(product, ct);
        await _uow.SaveChangesAsync(ct);

        return Result<Guid>.Success(product.Id);
    }
}