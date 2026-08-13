
using MediatR;
using ProductManagement.Application.DTOs;

public record GetOrderByIdQuery(Guid OrderId) : IRequest<OrderDto?>;