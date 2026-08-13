using MediatR;
using ProductManagement.Application.Interfaces;
using ProductManagement.Domain.Entities;
namespace ProductManagement.Application.Commands;

public class PlaceOrderCommandHandler : IRequestHandler<PlaceOrderCommand, Result<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;
    public PlaceOrderCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    

     public async Task<Result<Guid>> Handle(PlaceOrderCommand command, CancellationToken ct)
    {
        // Step A: does the product even exist?
        var product = await _unitOfWork.Products.GetByIdAsync(command.ProductId, ct);
        if (product is null)
            return Result<Guid>.Failure("Product not found.");

        // Step B: build the order
        ProductManagement.Domain.Entities.Order? order = new ProductManagement.Domain.Entities.Order(Guid.NewGuid(), command.ShippingAddress);

        try
        {
            // this checks stock internally (Domain's own rule) and adds the item
            order.AddItem(product.Id, command.Quantity, product.Price.Amount, product.StockQuantity);
        }
        catch (Exception ex)
        {
            return Result<Guid>.Failure(ex.Message);
        }

        // Step C: actually reduce the shelf count
        product.DecreaseStock(command.Quantity);
        await _unitOfWork.Products.UpdateAsync(product, ct);

        // Step D: save the order + ring the bell (raise the event)
        order.MarkPlaced();
        await _unitOfWork.Orders.AddAsync(order, ct);
        await _unitOfWork.SaveChangesAsync(ct);   // this is what actually dispatches OrderPlacedEvent

        return Result<Guid>.Success(order.Id);
    }
}
