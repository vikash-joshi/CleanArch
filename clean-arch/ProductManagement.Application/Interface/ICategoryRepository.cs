namespace ProductManagement.Application.Interfaces;

public interface ICategoryRepository
{
    Task<Category?> GetCategoryByIdAsync(Guid id, CancellationToken ct);
    Task<IEnumerable<Category>> GetAllCategoryAsync(CancellationToken ct);
    Task<IEnumerable<Product>> GetProductsByCategoryQuery(Guid categoryId, CancellationToken ct);
    Task<bool> ExistsCategoryByNameAsync(string Name, CancellationToken ct);
    
    Task AddCategoryAsync(Category category, CancellationToken ct);
    Task UpdateCategoryAsync(Category category, CancellationToken ct);
    Task DeleteCategoryAsync(Guid id, CancellationToken ct);
}