using MediatR;
namespace ProductManagement.Application.Commands;

public record CreateCategoryCommand(string Name, string Description)
    : IRequest<Result<Guid>>;