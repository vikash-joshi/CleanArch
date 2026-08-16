
using ProductManagement.Domain.Entities;
namespace ProductManagement.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> ExistByEmail(string Email, CancellationToken ct);
    Task AddAsync(User user, CancellationToken ct);
}