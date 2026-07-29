

using System.Security.Cryptography.X509Certificates;
using ProductManagement.Application.Interfaces;

namespace ProductManagement.Application.BusinessLogics;

public class ProductBusinessRules
{
    private readonly IProductRepository _repository;

    public ProductBusinessRules(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<string> EnsureNameIsUnique(string name, CancellationToken cancellationToken)
    {
        if (await _repository.ExistsByNameAsync(name, cancellationToken))
        {
            return ("Product name already exists.");
        }

        return "";

    }
}