    using ProductManagement.Application.Interfaces;

    namespace ProductManagement.Infrastructure;

    public sealed class InMemoryUnitOfWork : IUnitOfWork
    {
         
        public InMemoryUnitOfWork(IProductRepository products) => Products = products;


        public IProductRepository Products { get; }

        public Task<int> SaveChangesAsync(CancellationToken ct)
        {
            return Task.FromResult(1);
        }
    }

    public class InMemoryProductRepository : IProductRepository
{
    private readonly List<Product> _products = new();

    public Task<Product?> GetByIdAsync(Guid id, CancellationToken ct) =>
        Task.FromResult(_products.FirstOrDefault(p => p.Id == id));

    public Task<IEnumerable<Product>> GetByNameAsync(string Name, CancellationToken ct) => Task.FromResult(_products.Where(p => p.Name.ToLower().Contains(Name.ToLower())));
    

        
    
    public Task<IEnumerable<Product>> GetAllAsync(CancellationToken ct) =>
        Task.FromResult(_products.Where(x=>!x.IsDeleted).AsEnumerable());

    
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
}


  
