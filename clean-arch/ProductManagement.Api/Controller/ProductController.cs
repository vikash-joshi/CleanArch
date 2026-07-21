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

    [HttpPost("Update")]
    public async Task<IActionResult> Update(UpdateProductRequest req, CancellationToken ct)
    {
        var result = await _mediator.Send(
            new UpdateProductCommand(req.id, req.Name, req.Description, req.Price, req.StockQuantity), ct);

        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value }, result.Value)
            : BadRequest(result.Error);
    }

   [HttpDelete("Delete")]
    public async Task<IActionResult> Delete(DeleteProductRequest req, CancellationToken ct)
    {
        var result = await _mediator.Send(
            new DeleteProductCommand(new Guid(req.id),ct));

        return result.IsSuccess
            ? CreatedAtAction(nameof(GetById), new { id = result.Value }, result.Value)
            : BadRequest(result.Error);
    }


    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAllProducts(int page = 1, int pageSize = 2, string? search = null, CancellationToken ct = default)
    {
        var result = await _mediator.Send(new GetAllProductsQuery(page, pageSize, search, null), ct);
        return Ok(result);
    }

    [HttpGet("ById/{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var product = await _mediator.Send(new GetProductByIdQuery(id), ct);
        return product is not null ? Ok(product) : NotFound();
    }
    
    [HttpGet("ByName")]
    public async Task<IActionResult> GetByName([FromQuery] string name, CancellationToken ct)
    {
        var product = await _mediator.Send(new GetAllProductByNameQuery(name), ct);
        return product is not null ? Ok(product): NotFound("Not Found");
    }

}