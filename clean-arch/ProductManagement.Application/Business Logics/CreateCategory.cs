

using System.Security.Cryptography.X509Certificates;
using ProductManagement.Application.Interfaces;

namespace ProductManagement.Application.BusinessLogics;

public class CategoryBusinessRules
{
    private readonly ICategoryRepository _repository;

    public CategoryBusinessRules(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<string> EnsureNameIsUnique(string name, CancellationToken cancellationToken)
    {
        if (await _repository.ExistsCategoryByNameAsync(name, cancellationToken))
        {
            return ("Category name already exists.");
        }

        return "";

    }
}