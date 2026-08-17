

using System.Security.Cryptography;
using MediatR;
using ProductManagement.Application.Commands.Auth;
using ProductManagement.Application.Interfaces;

public class LoginCommandHanlder : IRequestHandler<LoginCommand,Result<LoginResult>>
{

    public readonly IUnitOfWork _uow;
    public readonly IJwtTokenGenerator tokenGenerator;
    public LoginCommandHanlder(IUnitOfWork _uow,IJwtTokenGenerator tokenGenerator)
    {
        this.tokenGenerator = tokenGenerator;
        this._uow = _uow;
    }
    public async Task<Result<LoginResult>> Handle(LoginCommand command,CancellationToken ct)
    {

        var user = await _uow.Users.ExistByEmail(command.Email,ct);
        if(user is null)
        {
            return Result<LoginResult>.Failure("User Not Exist By Given Email");
        }
        if(BCrypt.Net.BCrypt.Verify(command.Password,user.PasswordHash) == false)
        {
            return Result<LoginResult>.Failure("Invalid Password");
        }

        var token = tokenGenerator.GenerateToken(user.Id,user.Email,user.userRole.ToString());
        var refreshTokenValue = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        var refreshToken = new RefreshToken(user.Id, refreshTokenValue, DateTime.UtcNow.AddDays(7));
        
        await _uow.RefreshTokens.AddAsync(refreshToken, ct);
 await _uow.SaveChangesAsync(ct);

  
        return Result<LoginResult>.Success(new LoginResult(token, refreshTokenValue));
    }
}