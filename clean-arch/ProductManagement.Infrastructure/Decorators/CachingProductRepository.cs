using Microsoft.Extensions.Caching.Memory;
using ProductManagement.Application.Interfaces;


namespace ProductManagement.Infrastructure.Decorators;

public class CachingProductRepository : IProductRepository
{
    private readonly IProductRepository _inner;   // the real repository, wrapped
    private readonly IMemoryCache _cache;
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);

    public CachingProductRepository(IProductRepository inner, IMemoryCache cache)
    {
        _inner = inner;
        _cache = cache;
    }

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var cacheKey = $"product:{id}";

        if (_cache.TryGetValue(cacheKey, out Product? cached))
        {
            Console.WriteLine($"CACHE HIT: {id}");
            return cached;
        }

        Console.WriteLine($"CACHE MISS: {id}");
        var product = await _inner.GetByIdAsync(id, ct);   // delegate to the real repository

        if (product is not null)
            _cache.Set(cacheKey, product, CacheDuration);

        return product;
    }

    // Everything else just passes straight through — no caching needed for these
    public Task<IEnumerable<Product>> GetAllAsync(CancellationToken ct) => _inner.GetAllAsync(ct);
    public Task AddAsync(Product product, CancellationToken ct) => _inner.AddAsync(product, ct);

    public async Task UpdateAsync(Product product, CancellationToken ct)
    {
        await _inner.UpdateAsync(product, ct);
        _cache.Remove($"product:{product.Id}");   // invalidate stale cache entry
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct)
    {
        await _inner.DeleteAsync(id, ct);
        _cache.Remove($"product:{id}");   // invalidate here too
    }

    public Task<IEnumerable<Product>> GetByNameAsync(string Name, CancellationToken ct) =>
        _inner.GetByNameAsync(Name, ct);

    public Task<bool> ExistsByNameAsync(string Name, CancellationToken ct) =>
        _inner.ExistsByNameAsync(Name, ct);

    public Task AssignCategoryToProductAsync(Guid productId, Guid categoryId, CancellationToken ct) =>
        _inner.AssignCategoryToProductAsync(productId, categoryId, ct);
}