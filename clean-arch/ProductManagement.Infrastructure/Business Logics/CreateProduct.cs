

using ProductManagement.Application.Interfaces;

namespace ProductManagement.Infrastructure.BusinessLogics;

public class ProductBusinessRules
{
    private readonly IProductRepository _repository;

    public ProductBusinessRules(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task EnsureNameIsUnique(string name, CancellationToken cancellationToken)
    {
        if (await _repository.ExistsByNameAsync(name, cancellationToken))
        {
            throw new Exception("Product name already exists.");
        }
    }
}