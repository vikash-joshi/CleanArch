using ProductManagement.Application.Interfaces;

namespace ProductManagement.Infrastructure;

public sealed class InMemoryUnitOfWork : IUnitOfWork
{
    private readonly List<Product> _products = [];

    public InMemoryUnitOfWork()
    {
        Products = new InMemoryProductRepository(_products);
    }

    public IProductRepository Products { get; }

    public Task<int> SaveChangesAsync(CancellationToken ct)
    {
        return Task.FromResult(1);
    }
}

internal sealed class InMemoryProductRepository : IProductRepository
{
    private readonly List<Product> _products;

    public InMemoryProductRepository(List<Product> products)
    {
        _products = products;
    }

    public Task<Product?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return Task.FromResult(_products.FirstOrDefault(p => p.Id == id));
    }

    public Task<IEnumerable<Product>> GetAllAsync(CancellationToken ct)
    {
        return Task.FromResult<IEnumerable<Product>>(_products.ToList());
    }

    public Task AddAsync(Product product, CancellationToken ct)
    {
        _products.Add(product);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Product product, CancellationToken ct)
    {
        var existing = _products.FirstOrDefault(p => p.Id == product.Id);
        if (existing is not null)
        {
            _products.Remove(existing);
            _products.Add(product);
        }

        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id, CancellationToken ct)
    {
        var existing = _products.FirstOrDefault(p => p.Id == id);
        if (existing is not null)
        {
            _products.Remove(existing);
        }

        return Task.CompletedTask;
    }
}
