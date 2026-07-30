using MediatR;
using ProductManagement.Application.Interfaces;

public class GetAllProductsByCategoriesHandler : IRequestHandler<GetAllProductsByCategoriesQuery, PagedResult<ProductDto>>
{
    public readonly IUnitOfWork _uow;

    public GetAllProductsByCategoriesHandler(IUnitOfWork _uow)
    {
        this._uow = _uow;
    }

    public async Task<PagedResult<ProductDto>> Handle(GetAllProductsByCategoriesQuery request, CancellationToken ct)
    {
        var Data = await _uow.Categories.GetProductsByCategoryQuery(request.CategoryId, ct);

         var filtered = Data.AsEnumerable();

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