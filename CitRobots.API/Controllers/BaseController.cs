using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseController : ControllerBase
{
    protected readonly ILogger<BaseController> _logger;

    protected BaseController(ILogger<BaseController> logger)
    {
        _logger = logger;
    }

    protected ActionResult HandleException(Exception ex)
    {
        _logger.LogError(ex, ex.Message);

        return ex switch
        {
            NotFoundException => NotFound(ex.Message),
            UnauthorizedException => Unauthorized(ex.Message),
            BusinessException => BadRequest(ex.Message),
            _ => StatusCode(500, "Ocorreu um erro interno. Por favor, tente novamente mais tarde.")
        };
    }
}