using MediatR;

public record GetAllProductByNameQuery(string Name):IRequest<IEnumerable<ProductDto>>;
