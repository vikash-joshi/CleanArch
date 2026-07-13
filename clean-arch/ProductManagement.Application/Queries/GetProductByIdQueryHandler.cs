using MediatR;
using ProductManagement.Application.Interfaces;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
{
    private readonly IUnitOfWork _uow;

    public GetProductByIdQueryHandler(IUnitOfWork uow) => _uow = uow;

    public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken ct)
    {
        var product = await _uow.Products.GetByIdAsync(request.Id, ct);
        return product?.ToDto();
    }
}