using ProductManagement.Application.Interfaces;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProductManagement.Infrastructure;

public sealed class InMemoryUnitOfWork : IUnitOfWork
{
    public InMemoryUnitOfWork(IProductRepository products, ICategoryRepository categories)
    {
        Products = products;
        Categories = categories;
    }

    public IProductRepository Products { get; }

    public ICategoryRepository Categories { get; }

    public Task<int> SaveChangesAsync(CancellationToken ct)
    {
        return Task.FromResult(1);
    }
}

public class InMemoryProductRepository : IProductRepository, ICategoryRepository
{
    private readonly List<Product> _products = new();
    private readonly List<Category> _category = new();

    public Task<Product?> GetByIdAsync(Guid id, CancellationToken ct) =>
        Task.FromResult(_products.FirstOrDefault(p => p.Id == id));

    public Task<IEnumerable<Product>> GetByNameAsync(string Name, CancellationToken ct) => Task.FromResult(_products.Where(p => p.Name.ToLower().Contains(Name.ToLower())));
    
    public Task<IEnumerable<Product>> GetAllAsync(CancellationToken ct)
    {
        var Products = _products.Where(x => !x.IsDeleted).AsEnumerable();
        Console.WriteLine("Products Count: " + JsonSerializer.Serialize(Products));
        
        var Categories = _category.AsEnumerable();
Console.WriteLine("Products Count: " + JsonSerializer.Serialize(Categories));
        var final = Products.Join(Categories, p => p.CategoryId, c => c.Id, (p, c) =>
        {
            p.CategoryName = c.Name;
            
            return p;
        });
Console.WriteLine("Products Count: " + JsonSerializer.Serialize(final));
        


        return Task.FromResult(final.AsEnumerable());
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

    public async Task<bool> ExistsByNameAsync(string Name, CancellationToken ct)
    {
        return _products.Any(p => p.Name == Name);
    }
    
    public async Task<bool> ExistsCategoryByNameAsync(string Name, CancellationToken ct)
    {
        return _category.Any(c => c.Name == Name);
    }
    
    public Task<Category?> GetCategoryByIdAsync(Guid id, CancellationToken ct)
    {
        return Task.FromResult(_category.FirstOrDefault(p => p.Id == id));
    }
    public Task<IEnumerable<Category>> GetAllCategoryAsync(CancellationToken ct) => Task.FromResult(_category.AsEnumerable());
    public Task AddCategoryAsync(Category category, CancellationToken ct)
    {
        _category.Add(category);
        return Task.CompletedTask;
    }
    public Task UpdateCategoryAsync(Category category, CancellationToken ct)
    {
        var existIndex = _category.FindIndex(p => p.Id == category.Id);
        if (existIndex >= 0) _category[existIndex] = category;
        return Task.CompletedTask;
    }
    public Task DeleteCategoryAsync(Guid id, CancellationToken ct)
    {
        var index = _products.FindIndex(p => p.CategoryId == id);
        if (index >= 0)
        {
            return Task.FromException(new InvalidOperationException("Cannot delete category because it is associated with existing products."));
        }
        _category.RemoveAll(p => p.Id == id);

        return Task.CompletedTask;
    }

    public Task<IEnumerable<Product>> GetProductsByCategoryQuery(Guid categoryId, CancellationToken ct)
    {
        var productByCategory = _products.Where(p => !p.IsDeleted && p.CategoryId == categoryId).ToList();
        return Task.FromResult(productByCategory.AsEnumerable());
    }

    public Task AssignCategoryToProductAsync(Guid productId, Guid categoryId, CancellationToken ct)
    {
        var product = _products.FirstOrDefault(p => p.Id == productId);
        if (product != null)
        {
            product.AssignCategory(categoryId);  
        }
        return Task.CompletedTask;
    }

}



