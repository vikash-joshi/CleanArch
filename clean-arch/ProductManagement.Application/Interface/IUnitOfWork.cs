namespace ProductManagement.Application.Interfaces;

public interface IUnitOfWork
{
    IProductRepository Products { get;set; }

    ICategoryRepository Categories { get; }
    Task<int> SaveChangesAsync(CancellationToken ct);
}