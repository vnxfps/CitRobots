using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    private readonly IRoboService _roboService;

    public HomeController(IRoboService roboService)
    {
        _roboService = roboService;
    }

    public async Task<IActionResult> Index()
    {
        var robos = await _roboService.GetAllRobosAsync();
        return View(robos);
    }
}