using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using ProductManagement.Domain.Entities;
using ProductManagement.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace ProductManagement.Application.Commands.Auth;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<Guid>>
{
    public readonly IUnitOfWork _uow;

    public readonly ILogger<RegisterCommand> logger;

    public RegisterCommandHandler(IUnitOfWork _uow,ILogger<RegisterCommand> logger) 
    {
        this._uow = _uow;
        this.logger = logger;
    }

    public async Task<Result<Guid>> Handle(RegisterCommand command, CancellationToken ct)
    {
        if (await _uow.Users.ExistByEmail(command.Email, ct) is not null)
            return Result<Guid>.Failure("User Email Already Exists");


        var hash = BCrypt.Net.BCrypt.HashPassword(command.Password);
        logger.LogInformation($"Role {command.Role}");
        
        Enum.TryParse(command.Role, true, out UserRole role);
        var user = new User(Guid.NewGuid(), command.Email, hash,string.IsNullOrEmpty(command.Role) ? UserRole.Customer : role);
        logger.LogInformation($"Role {user.userRole}");
        await _uow.Users.AddAsync(user, ct);
        await _uow.SaveChangesAsync(ct);

        return Result<Guid>.Success(user.Id);

    }

}
