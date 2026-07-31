using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.Application.Commands;

[ApiController]
[Route("api/v1/category")]
public class CategoryController : ControllerBase
{

    private readonly IMediator _mediator;

    public CategoryController(IMediator _mediator) => this._mediator = _mediator;
    [HttpGet("Index")]
    public async Task<IActionResult> Index()
    {
        return Ok("Category Controller");
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create(CreateCategoryRequest request,CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CreateCategoryCommand(request.Name, request.Description), cancellationToken);

        return result.IsSuccess ? Ok("Create Category "+ result.Value) : BadRequest("Failed to create category: " + result.Error);
    }

    [HttpPost("Update")]
    public async Task<IActionResult> Update(UpdateCategoryRequest request,CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new UpdateCategoryCommand(new Guid(request.id), request.Name, request.Description), cancellationToken);

        return result.IsSuccess ? Ok("Update Category") : BadRequest("Failed to update category: " + result.Error);
    }

    [HttpPost("Delete")]
    public async Task<IActionResult> Delete(DeleteCategoryRequest request,CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new DeleteCategoryCommand(new Guid(request.id), cancellationToken));
        return result.IsSuccess ? Ok("Delete Category") : BadRequest("Failed to delete category: " + result.Error);
    }

    [HttpGet("GetCategory")]
    public async Task<IActionResult> GetCategory(int Page = 1, int PageSize = 10, string? Search = null, CancellationToken cancellationToken = default)
    {
        var result = await _mediator.Send(new GetAllCategoriesQuery(Page, PageSize, Search), cancellationToken);
        return Ok(result);
    }
}