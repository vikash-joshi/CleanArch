using MediatR;
using ProductManagement.Application.Interfaces;
using ProductManagement.Domain.Common;

namespace ProductManagement.Infrastructure
{

public sealed class InMemoryDatabase
{
    public List<Product> Products { get; } = new();
    public List<Category> Categories { get; } = new();
}

public sealed class InMemoryUnitOfWork : IUnitOfWork
{
    private readonly IDomainEventDispatcher _dispatcher;
    private readonly List<Product> _products;
    private readonly List<Category> _categories;

    public InMemoryUnitOfWork(InMemoryDatabase database, IDomainEventDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
        _products = database.Products;
        _categories = database.Categories;
        Products = new InMemoryProductRepository(_products, _categories);
        Categories = new InMemoryCategoryRepository(_categories, _products);
    }

    public IProductRepository Products { get; set; }
    public ICategoryRepository Categories { get; }

    public async Task<int> SaveChangesAsync(CancellationToken ct)
    {
        await _dispatcher.DispatchAndClearEvents(_products.OfType<Entity>(), ct);
        return 1;
    }
}

public sealed class InMemoryProductRepository : IProductRepository
{
    private readonly List<Product> _products;
    private readonly List<Category> _categories;   // needed only for the join in GetAllAsync

    public InMemoryProductRepository(List<Product> products, List<Category> categories)
    {
        _products = products;
        _categories = categories;
    }

    public Task<Product?> GetByIdAsync(Guid id, CancellationToken ct) =>
        Task.FromResult(_products.FirstOrDefault(p => p.Id == id));

    public Task<IEnumerable<Product>> GetByNameAsync(string name, CancellationToken ct) =>
        Task.FromResult(_products.Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase)));

    public Task<IEnumerable<Product>> GetAllAsync(CancellationToken ct)
    {
        var active = _products.Where(p => !p.IsDeleted);

        // ⚠️ see LEFT JOIN note below — this fixes a real bug in your version
        var joined = from p in active
                     join c in _categories on p.CategoryId equals c.Id into cg
                     from c in cg.DefaultIfEmpty()
                     select AttachCategoryName(p, c);

        return Task.FromResult(joined);
    }

    private static Product AttachCategoryName(Product p, Category? c)
    {
        p.CategoryName = c?.Name;   // null if product has no category — that's fine now
        return p;
    }

    public Task AddAsync(Product product, CancellationToken ct)
    {
        _products.Add(product);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(Product product, CancellationToken ct)
    {
        var index = _products.FindIndex(p => p.Id == product.Id);
        if (index >= 0) _products[index] = product;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id, CancellationToken ct)
    {
        _products.RemoveAll(p => p.Id == id);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsByNameAsync(string name, CancellationToken ct) =>
        Task.FromResult(_products.Any(p => p.Name == name));

    public Task<IEnumerable<Product>> GetProductsByCategoryQuery(Guid categoryId, CancellationToken ct) =>
        Task.FromResult(_products.Where(p => !p.IsDeleted && p.CategoryId == categoryId).AsEnumerable());

    public Task AssignCategoryToProductAsync(Guid productId, Guid categoryId, CancellationToken ct)
    {
        var product = _products.FirstOrDefault(p => p.Id == productId);
        product?.AssignCategory(categoryId);
        return Task.CompletedTask;
    }
}

public sealed class InMemoryCategoryRepository : ICategoryRepository
{
    private readonly List<Category> _categories;
    private readonly List<Product> _products;   // needed only for the "in-use" check on delete

    public InMemoryCategoryRepository(List<Category> categories, List<Product> products)
    {
        _categories = categories;
        _products = products;
    }

    public Task<Category?> GetCategoryByIdAsync(Guid id, CancellationToken ct) =>
        Task.FromResult(_categories.FirstOrDefault(c => c.Id == id));

    public Task<IEnumerable<Category>> GetAllCategoryAsync(CancellationToken ct) =>
        Task.FromResult(_categories.AsEnumerable());

    public Task AddCategoryAsync(Category category, CancellationToken ct)
    {
        _categories.Add(category);
        return Task.CompletedTask;
    }

    public Task UpdateCategoryAsync(Category category, CancellationToken ct)
    {
        var index = _categories.FindIndex(c => c.Id == category.Id);
        if (index >= 0) _categories[index] = category;
        return Task.CompletedTask;
    }

    public Task DeleteCategoryAsync(Guid id, CancellationToken ct)
    {
        if (_products.Any(p => p.CategoryId == id))
            return Task.FromException(
                new InvalidOperationException("Cannot delete category because it is associated with existing products."));

        _categories.RemoveAll(c => c.Id == id);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsCategoryByNameAsync(string name, CancellationToken ct) =>
        Task.FromResult(_categories.Any(c => c.Name == name));

    public Task<IEnumerable<Product>> GetProductsByCategoryQuery(Guid categoryId, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
}