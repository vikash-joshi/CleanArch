using ProductManagement.Domain.Entities;

namespace ProductManagement.Application.Interfaces;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken ct);
    Task AddAsync(RefreshToken refreshToken, CancellationToken ct);
}