using MediatR;

namespace ProductManagement.Application.Commands;

public record PlaceOrderCommand(string ShippingAddress, Guid ProductId, int Quantity)
    : IRequest<Result<Guid>>;