using System.Diagnostics;
using Microsoft.Extensions.Logging;
using ProductManagement.Application.Interfaces;


namespace ProductManagement.Infrastructure.Decorators;

public class LoggingProductRepository : IProductRepository
{
    private readonly IProductRepository _inner;
    private readonly ILogger<LoggingProductRepository> _logger;

    public LoggingProductRepository(IProductRepository inner, ILogger<LoggingProductRepository> logger)
    {
        _inner = inner;
        _logger = logger;
    }

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var sw = Stopwatch.StartNew();
        var result = await _inner.GetByIdAsync(id, ct);
        sw.Stop();
        _logger.LogInformation("GetByIdAsync({Id}) took {Ms}ms", id, sw.ElapsedMilliseconds);
        return result;
    }

    public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken ct)
    {
        var sw = Stopwatch.StartNew();
        var result = await _inner.GetAllAsync(ct);
        sw.Stop();
        _logger.LogInformation("GetAllAsync() took {Ms}ms, returned {Count} items", sw.ElapsedMilliseconds, result.Count());
        return result;
    }

    public async Task AddAsync(Product product, CancellationToken ct)
    {
        var sw = Stopwatch.StartNew();
        _logger.LogInformation("AddAsync");
        await _inner.AddAsync(product, ct);
        sw.Stop();
        _logger.LogInformation("AddAsync({Id}) took {Ms}ms", product.Id, sw.ElapsedMilliseconds);
    }

    public async Task UpdateAsync(Product product, CancellationToken ct)
    {
        var sw = Stopwatch.StartNew();
        await _inner.UpdateAsync(product, ct);
        sw.Stop();
        _logger.LogInformation("UpdateAsync({Id}) took {Ms}ms", product.Id, sw.ElapsedMilliseconds);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct)
    {
        
        var sw = Stopwatch.StartNew();
        await _inner.DeleteAsync(id, ct);
        sw.Stop();
        _logger.LogInformation("DeleteAsync({Id}) took {Ms}ms", id, sw.ElapsedMilliseconds);
    }

    public Task<IEnumerable<Product>> GetByNameAsync(string Name, CancellationToken ct)
    {
        _logger.LogInformation("GetByNameAsync");
        return _inner.GetByNameAsync(Name, ct);
    }

    public Task<bool> ExistsByNameAsync(string Name, CancellationToken ct)
    {
        _logger.LogInformation("ExistsByNameAsync");
        return _inner.ExistsByNameAsync(Name, ct);
    }

    public Task AssignCategoryToProductAsync(Guid productId, Guid categoryId, CancellationToken ct)
    {
        _logger.LogInformation("AssignCategoryToProductAsync");
        return _inner.AssignCategoryToProductAsync(productId, categoryId, ct);
    }
}