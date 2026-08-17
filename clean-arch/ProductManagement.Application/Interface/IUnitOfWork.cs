using ProductManagement.Application.Interface;
namespace ProductManagement.Application.Interfaces;

public interface IUnitOfWork
{
    IProductRepository Products { get; set; }
    IOrderRepository Orders { get; set; }
    ICategoryRepository Categories { get; set; }

    IUserRepository Users {get;set;}

    IRefreshTokenRepository RefreshTokens { get;set; }   // 🆕

    Task<int> SaveChangesAsync(CancellationToken ct);
}