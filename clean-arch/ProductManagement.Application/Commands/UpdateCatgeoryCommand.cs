using MediatR;
namespace ProductManagement.Application.Commands;

public record UpdateCategoryCommand(Guid Id, string Name, string Description)
    : IRequest<Result<bool>>;