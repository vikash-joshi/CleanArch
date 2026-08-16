using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using ProductManagement.Domain.Entities;
using ProductManagement.Application.Interfaces;

namespace ProductManagement.Application.Commands.Auth;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<Guid>>
{
    public readonly IUnitOfWork _uow;

    public RegisterCommandHandler(IUnitOfWork _uow) => this._uow = _uow;

    public async Task<Result<Guid>> Handle(RegisterCommand command, CancellationToken ct)
    {
        if (await _uow.Users.ExistByEmail(command.Email, ct) is not null)
            return Result<Guid>.Failure("User Email Already Exists");


        var hash = BCrypt.Net.BCrypt.HashPassword(command.Password);
        var user = new User(Guid.NewGuid(), command.Email, hash);

        await _uow.Users.AddAsync(user, ct);
        await _uow.SaveChangesAsync(ct);

        return Result<Guid>.Success(user.Id);

    }

}
