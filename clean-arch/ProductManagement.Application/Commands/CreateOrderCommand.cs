using MediatR;
namespace ProductManagement.Application.Commands;

public record CreateOrderComamnd(string ProductId, int Quantity)
    : IRequest<Result<decimal>>;