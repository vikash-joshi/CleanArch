using MediatR;

namespace ProductManagement.Application.Commands.Auth;

public record RefreshTokenCommand(string RefreshToken) : IRequest<Result<string>>;