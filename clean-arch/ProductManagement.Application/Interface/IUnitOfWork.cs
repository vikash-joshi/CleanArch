namespace ProductManagement.Application.Interfaces;

public interface IUnitOfWork
{
    IProductRepository Products { get; }
    Task<int> SaveChangesAsync(CancellationToken ct);
}