using MediatR;

public record GetAllProductsByCategoriesQuery(Guid CategoryId, int Page, int PageSize, string? SearchTerm)
    : IRequest<PagedResult<ProductDto>>;