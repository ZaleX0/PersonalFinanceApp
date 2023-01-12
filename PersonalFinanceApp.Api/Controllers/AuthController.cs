using Microsoft.AspNetCore.Mvc;
using PersonalFinanceApp.Services.Interfaces;
using PersonalFinanceApp.Services.Models;

namespace PersonalFinanceApp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _service;

    public AuthController(IAuthService service)
    {
        _service = service;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserDto dto)
    {
        await _service.Register(dto);
        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserDto dto)
    {
        var token = await _service.CreateToken(dto);
        return Ok(token);
    }
}
