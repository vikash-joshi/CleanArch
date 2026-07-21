using MediatR;
namespace ProductManagement.Application.Commands;

public record UpdateProductCommand(string id, string Name, string Description, decimal Price, int Stock)
    : IRequest<Result<bool>>;