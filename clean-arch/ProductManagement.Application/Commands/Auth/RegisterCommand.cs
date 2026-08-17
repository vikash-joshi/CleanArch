using MediatR;

namespace ProductManagement.Application.Commands.Auth;

public record RegisterCommand(string Email, string Password,string? Role) : IRequest<Result<Guid>>;