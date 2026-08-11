using MediatR;
using ProductManagement.Application.DTOs;

public record GetProductByIdQuery(Guid Id) : IRequest<ProductDetailDto?>;
