using MediatR;

public record GetProductByIdQuery(Guid Id) : IRequest<ProductDto?>;
