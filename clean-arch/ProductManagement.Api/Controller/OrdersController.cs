using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.Application.Commands;

[ApiController]
[Route("api/v1/orders")]
public class OrdersController : ControllerBase
{

    private readonly IMediator _mediator;

    public OrdersController(IMediator _mediator) => this._mediator = _mediator;
    [HttpGet("Index")]
    public async Task<IActionResult> Index()
    {
        return Ok("Orders Controller");
    }

    [HttpPost("Place")]
    public async Task<IActionResult> Create(PlaceOrderCommand request,CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);

        return result.IsSuccess ? Ok("Create Order "+ result.Value) : BadRequest("Failed to create order: " + result.Error);
    }

    [HttpGet("GetOrder/{OrderId}")]
    public async Task<IActionResult> GetOrder(string OrderId,CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetOrderByIdQuery(new Guid(OrderId)), cancellationToken);

        return  Ok(result);
    }
}