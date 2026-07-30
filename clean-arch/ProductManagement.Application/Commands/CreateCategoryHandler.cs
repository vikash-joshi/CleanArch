using MediatR;
using Microsoft.Extensions.Logging;
using ProductManagement.Application.BusinessLogics;
using ProductManagement.Application.Interfaces;
namespace ProductManagement.Application.Commands;

public class CreateCategoryCommandHandler: IRequestHandler<CreateCategoryCommand,Result<Guid>>
{
    private readonly IUnitOfWork _uow;
    private readonly CategoryBusinessRules _rules;
    private readonly ILogger<CreateCategoryCommandHandler> _logger;

    public CreateCategoryCommandHandler(IUnitOfWork uow, CategoryBusinessRules rules, ILogger<CreateCategoryCommandHandler> logger)
    {
        _uow = uow;
        _rules = rules;
        _logger = logger;
    }

    public async Task<Result<Guid>> Handle(CreateCategoryCommand req,CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(req.Name))
            return Result<Guid>.Failure("Name is required.");

        var uniqueMessage = await _rules.EnsureNameIsUnique(req.Name, ct);
        if (!string.IsNullOrWhiteSpace(uniqueMessage))
            return Result<Guid>.Failure(uniqueMessage);

        var category = new Category(Guid.NewGuid(), req.Name, req.Description);
        await _uow.Categories.AddCategoryAsync(category, ct);
        await _uow.SaveChangesAsync(ct);
        return Result<Guid>.Success(category.Id);
    }
}