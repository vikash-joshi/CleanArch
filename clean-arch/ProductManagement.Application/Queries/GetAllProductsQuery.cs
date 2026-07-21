using MediatR;

public record GetAllProductsQuery(int Page, int PageSize, string? SearchTerm, Guid? CategoryId)
    : IRequest<PagedResult<ProductDto>>;