using MediatR;

public record UpdateCategoryCommand(Guid Id, string Name, string Description)
    : IRequest<Result<bool>>;