using MediatR;
using Microsoft.Extensions.Logging;
using ProductManagement.Application.BusinessLogics;
using ProductManagement.Application.Interfaces;
using ProductManagement.Domain.Factories;
namespace ProductManagement.Application.Commands;


public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<Guid>>
{
    private readonly IUnitOfWork _uow;

    private readonly ILogger<CreateProductCommandHandler> _logger;

    private readonly ProductBusinessRules Rules;



    public CreateProductCommandHandler(IUnitOfWork uow, ProductBusinessRules Rules, ILogger<CreateProductCommandHandler> _logger)
    {
        _uow = uow;
        this.Rules = Rules;
        this._logger = _logger;
    }

    public async Task<Result<Guid>> Handle(CreateProductCommand command, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(command.Name))
        {
            _logger.LogWarning("Name is required");
            return Result<Guid>.Failure("Name is required.");
        }

        if (command.Price < 0)
        {
            _logger.LogWarning("Price cannot be negative.");
            return Result<Guid>.Failure("Price cannot be negative.");
        }

        var result = await Rules.EnsureNameIsUnique(command.Name, ct);
        if (!string.IsNullOrEmpty(result))
        {
            return Result<Guid>.Failure(result);
        }

        Product product;
        try
        {
            product = ProductFactory.Create(command.Name, command.Description, command.Price, command.Stock);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning("Product creation failed: {Reason}", ex.Message);
            return Result<Guid>.Failure(ex.Message);
        }

        

        await _uow.Products.AddAsync(product, ct);
        await _uow.SaveChangesAsync(ct);
        
        _logger.LogInformation("Product created: {ProductId} - {Name}", product.Id, product.Name);


        return Result<Guid>.Success(product.Id);
    }
}