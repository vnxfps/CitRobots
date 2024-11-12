using Microsoft.AspNetCore.Mvc;
using CitRobots.Models;

namespace CitRobots.Controllers
{
    public class HomeController : Controller
    {
        private readonly CitRobotsContext _context;

        public HomeController(CitRobotsContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var robots = _context.Robots.Take(3).ToList();
            return View(robots);
        }
    }
}