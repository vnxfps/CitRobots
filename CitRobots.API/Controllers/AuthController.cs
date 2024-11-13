using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

public class AuthController : BaseController
{
    private readonly IAuthenticationService _authService;

    public AuthController(
        IAuthenticationService authService,
        ILogger<AuthController> logger) : base(logger)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDTO>> Login(LoginDTO loginDTO)
    {
        try
        {
            var token = await _authService.AuthenticateAsync(loginDTO.Email, loginDTO.Senha);
            return Ok(new AuthResponseDTO { Token = token });
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}