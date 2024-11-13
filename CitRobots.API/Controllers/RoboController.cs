using Microsoft.AspNetCore.Mvc;

public class RoboController : BaseController
{
    private readonly IRoboService _roboService;

    public RoboController(
        IRoboService roboService,
        ILogger<RoboController> logger) : base(logger)
    {
        _roboService = roboService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoboDTO>>> GetAll()
    {
        try
        {
            var robos = await _roboService.GetAllAsync();
            return Ok(robos.Select(r => new RoboDTO(r)));
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    [HttpGet("available")]
    public async Task<ActionResult<IEnumerable<RoboDTO>>> GetAvailable()
    {
        try
        {
            var robos = await _roboService.GetAvailableRobotsAsync();
            return Ok(robos.Select(r => new RoboDTO(r)));
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RoboDTO>> GetById(int id)
    {
        try
        {
            var robo = await _roboService.GetByIdAsync(id);
            return Ok(new RoboDTO(robo));
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }
}