using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.Application.Commands;

[ApiController]
[Route("api/v1/products")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductRequest req, CancellationToken ct)
    {
        var result = await _mediator.Send(
            new CreateProductCommand(req.Name, req.Description, req.Price, req.StockQuantity), ct);

        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value }, result.Value)
            : BadRequest(result.Error);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var product = await _mediator.Send(new GetProductByIdQuery(id), ct);
        return product is not null ? Ok(product) : NotFound();
    }
}