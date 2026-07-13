namespace ProductManagement.Application.Interfaces;
public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<IEnumerable<Product>> GetAllAsync(CancellationToken ct);
    Task AddAsync(Product product, CancellationToken ct);
    Task UpdateAsync(Product product, CancellationToken ct);
    Task DeleteAsync(Guid id, CancellationToken ct);
}