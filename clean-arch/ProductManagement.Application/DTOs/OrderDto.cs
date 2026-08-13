using MediatR;

namespace ProductManagement.Application.DTOs;

public record OrderDto(Guid Id, string ShippingAddress, string Status):IRequest<Result<OrderDto>>;