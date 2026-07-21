using MediatR;
using ProductManagement.Application.Interfaces;

public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, PagedResult<ProductDto>>
{
    private readonly IUnitOfWork _uow;

    public GetAllProductsQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<PagedResult<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken ct)
    {
        var allProducts = await _uow.Products.GetAllAsync(ct);

        var filtered = allProducts.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            filtered = filtered.Where(p =>
                p.Name.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase));

        var totalCount = filtered.Count();

        var paged = filtered
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(p => p.ToDto());

        return new PagedResult<ProductDto>
        {
            Items = paged,
            TotalCount = totalCount,
            PageNumber = request.Page,
            PageSize = request.PageSize
        };
    }
}