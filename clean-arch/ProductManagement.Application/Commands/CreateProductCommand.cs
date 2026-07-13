using MediatR;
namespace ProductManagement.Application.Commands;

public record CreateProductCommand(string Name, string Description, decimal Price, int Stock)
    : IRequest<Result<Guid>>;