

using MediatR;

public record DeleteCategoryCommand(Guid Id,CancellationToken ct) : IRequest<Result<bool>>;
