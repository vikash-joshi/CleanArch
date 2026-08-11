using MediatR;
using ProductManagement.Application.DTOs;

public record GetAllProductsQuery(int Page, int PageSize, string? SearchTerm, Guid? CategoryId)
    : IRequest<PagedResult<ProductListItemDto>>;