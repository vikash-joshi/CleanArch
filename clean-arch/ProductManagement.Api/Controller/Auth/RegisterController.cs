using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductManagement.Application.Commands.Auth;

[ApiController]
[Route("api/v1/register")]
public class RegisterController : ControllerBase
{
    private readonly IMediator mediator;

    public RegisterController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterCommand com,CancellationToken ct)
    {
        var result = await mediator.Send(com,ct);
        return result.IsSuccess ? Ok("User Created " +result.Value) : NotFound(result.Error);
    }

     [HttpPost("login")]
    public async Task<IActionResult> Login(LoginCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return result.IsSuccess ? Ok(new { token = result.Value }) : Unauthorized(result.Error);
    }

    [HttpGet("Users")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAllUsersQuery(""),cancellationToken);

        return Ok(result);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshTokenCommand command, CancellationToken ct)
    {
        var result = await mediator.Send(command, ct);
        return result.IsSuccess ? Ok(new { accessToken = result.Value }) : Unauthorized(result.Error);
    }

}