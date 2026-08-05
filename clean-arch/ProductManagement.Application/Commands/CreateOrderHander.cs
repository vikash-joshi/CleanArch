using MediatR;
using Microsoft.Extensions.Logging;
using ProductManagement.Application.BusinessLogics;
using ProductManagement.Application.Interfaces;
using ProductManagement.Application.Strategies;
namespace ProductManagement.Application.Commands;


public class CreateOrderCommandHandler : IRequestHandler<CreateOrderComamnd, Result<decimal>>
{
    private readonly IUnitOfWork _uow;

    private readonly ILogger<CreateOrderCommandHandler> _logger;

    private readonly  PricingStrategyFactory PSF;



    public CreateOrderCommandHandler(IUnitOfWork uow, PricingStrategyFactory PSF, ILogger<CreateOrderCommandHandler> _logger)
    {
        this.PSF = PSF;
        _uow = uow;
        this._logger = _logger;
    }

    public async Task<Result<decimal>> Handle(CreateOrderComamnd command, CancellationToken ct)
    {

        var product = await _uow.Products.GetByIdAsync(new Guid(command.ProductId), ct);

        if (product is null)
        {
            return Result<decimal>.Failure("product not found");
        }

         var strategy = PSF.GetStrategy(command.Quantity);

        var FinalPrice = strategy.CalculatePrice(product,command.Quantity);
        //_logger.LogInformation("Product created: {ProductId} - {Name}", product.Id, product.Name);


        return Result<decimal>.Success(FinalPrice);
    }
}