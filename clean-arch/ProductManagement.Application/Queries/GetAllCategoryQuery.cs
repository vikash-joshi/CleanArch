using MediatR;

public record GetAllCategoriesQuery(int Page, int PageSize, string? SearchTerm)
    : IRequest<PagedResult<CategoryDTO>>;