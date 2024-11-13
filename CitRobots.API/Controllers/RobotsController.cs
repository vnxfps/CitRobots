using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CitRobots.Models;

public class RobotsController : Controller
{
    private readonly CitRobotsContext _context;

    public RobotsController(CitRobotsContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Details(int id)
    {
        var robot = await _context.Robots
            .Include(r => r.Personalizacoes)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (robot == null)
        {
            return NotFound();
        }

        return View(robot);
    }

    public async Task<IActionResult> Customize(int id)
    {
        var robot = await _context.Robots.FindAsync(id);
        if (robot == null)
        {
            return NotFound();
        }

        var personalizacoes = await _context.Personalizacoes.ToListAsync();

        var viewModel = new CustomizeViewModel
        {
            Robot = robot,
            Personalizacoes = personalizacoes
        };

        return View(viewModel);
    }
}
