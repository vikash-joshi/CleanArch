using MediatR;

namespace ProductManagement.Application.Commands.Auth;

public record LoginCommand(string Email, string Password) : IRequest<Result<LoginResult>>;

public record LoginResult(string AccessToken, string RefreshToken);
