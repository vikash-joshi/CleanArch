using MediatR;
namespace ProductManagement.Application.Commands;
public record AssignCategoryToProductCommand(Guid ProductId, Guid CategoryId) : IRequest<Result<bool>>;