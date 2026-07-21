

using MediatR;

public record DeleteProductCommand(Guid Id,CancellationToken ct) : IRequest<Result<bool>>;