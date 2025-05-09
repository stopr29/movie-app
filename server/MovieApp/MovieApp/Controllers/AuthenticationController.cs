using Microsoft.AspNetCore.Mvc;
using MovieApp.Domain.Commands.Users;
using MovieApp.Services.Users;

namespace MovieApp.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserCommand command, CancellationToken token)
    {
        var result = await _authService.RegisterAsync(command, token);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserCommand command, CancellationToken token)
    {
        var result = await _authService.LoginAsync(command, token);
        return Ok(result);
    }
}