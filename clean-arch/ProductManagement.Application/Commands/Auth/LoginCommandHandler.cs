

using MediatR;
using ProductManagement.Application.Commands.Auth;
using ProductManagement.Application.Interfaces;

public class LoginCommandHanlder : IRequestHandler<LoginCommand,Result<string>>
{

    public readonly IUnitOfWork _uow;
    public readonly IJwtTokenGenerator tokenGenerator;
    public LoginCommandHanlder(IUnitOfWork _uow,IJwtTokenGenerator tokenGenerator)
    {
        this.tokenGenerator = tokenGenerator;
        this._uow = _uow;
    }
    public async Task<Result<string>> Handle(LoginCommand command,CancellationToken ct)
    {

        var user = await _uow.Users.ExistByEmail(command.Email,ct);
        if(user is null)
        {
            return Result<string>.Failure("User Not Exist By Given Email");
        }
        if(BCrypt.Net.BCrypt.Verify(command.Password,user.PasswordHash) == false)
        {
            return Result<string>.Failure("Invalid Password");
        }

        var token = tokenGenerator.GenerateToken(user.Id,user.Email);
        return Result<string>.Success(token);
    }
}