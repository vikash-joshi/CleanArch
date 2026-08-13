

using MediatR;
using ProductManagement.Application.DTOs;
using ProductManagement.Application.Interface;
using ProductManagement.Application.Interfaces;

public class GetOrderByIdQueryHanlder : IRequestHandler<GetOrderByIdQuery,OrderDto?>
{
    public readonly IUnitOfWork _orderRepository;
    public GetOrderByIdQueryHanlder(IUnitOfWork _orderRepository)
    {
        this._orderRepository = _orderRepository;
    }

    public async Task<OrderDto?> Handle(GetOrderByIdQuery req,CancellationToken ct)
    {
        var order = await _orderRepository.Orders.GetByIdAsync(req.OrderId, ct);
        
        if(order is null) return new OrderDto(req.OrderId, string.Empty, string.Empty);
        
        return new OrderDto(order.Id, order.ShippingAddress, order.Status.ToString());
    }

}