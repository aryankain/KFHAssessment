using KFHAssessment.Server.Services;
using KFHAssessment.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace KFHAssessment.Server.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _auth;
    public AuthController(AuthService auth) => _auth = auth;

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginRequestDto dto)
    {
        var result = await _auth.LoginAsync(dto);
        return result is null ? Unauthorized("Invalid credentials.") : Ok(result);
    }
}