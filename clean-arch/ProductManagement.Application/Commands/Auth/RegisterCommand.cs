using MediatR;

namespace ProductManagement.Application.Commands.Auth;

public record RegisterCommand(string Email, string Password) : IRequest<Result<Guid>>;