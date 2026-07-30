namespace ProductManagement.Application.Interfaces;

public interface IUnitOfWork
{
    IProductRepository Products { get; }

    ICategoryRepository Categories { get; }
    Task<int> SaveChangesAsync(CancellationToken ct);
}