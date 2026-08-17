using MediatR;
using ProductManagement.Application.Interfaces;

namespace ProductManagement.Application.Commands.Auth;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<string>>
{
    private readonly IUnitOfWork _uow;
    private readonly IJwtTokenGenerator _tokenGenerator;

    public RefreshTokenCommandHandler(IUnitOfWork uow, IJwtTokenGenerator tokenGenerator)
    {
        _uow = uow;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<Result<string>> Handle(RefreshTokenCommand command, CancellationToken ct)
    {
        var storedToken = await _uow.RefreshTokens.GetByTokenAsync(command.RefreshToken, ct);
        if (storedToken is null || !storedToken.IsActive)
            return Result<string>.Failure("Invalid or expired refresh token.");

        var user = await _uow.Users.GetUserById(storedToken.UserId, ct);
        if (user is null)
            return Result<string>.Failure("User not found.");

        var newAccessToken = _tokenGenerator.GenerateToken(user.Id, user.Email, user.userRole.ToString());
        return Result<string>.Success(newAccessToken);
    }
}