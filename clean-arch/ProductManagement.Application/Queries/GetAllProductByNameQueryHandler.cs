using MediatR;
using ProductManagement.Application.Interfaces;

public class GetAllProductByNameQueryHandler : IRequestHandler<GetAllProductByNameQuery, IEnumerable<ProductDto>>
{
    private readonly IUnitOfWork _unow;
    
    public GetAllProductByNameQueryHandler(IUnitOfWork _unow) => this._unow = _unow;

    public async Task<IEnumerable<ProductDto>> Handle(GetAllProductByNameQuery req, CancellationToken ct)
    {
        var products = await _unow.Products.GetByNameAsync(req.Name, ct);
        return products.Select(p => p.ToDto());
    }

}      