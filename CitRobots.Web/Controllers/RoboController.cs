using Microsoft.AspNetCore.Mvc;

public class RoboController : Controller
{
    private readonly IRoboService _roboService;
    private readonly IPersonalizacaoService _personalizacaoService;

    public RoboController(
        IRoboService roboService,
        IPersonalizacaoService personalizacaoService)
    {
        _roboService = roboService;
        _personalizacaoService = personalizacaoService;
    }

    public async Task<IActionResult> Detalhes(int id)
    {
        var robo = await _roboService.GetRoboDetalhesAsync(id);
        if (robo == null) return NotFound();

        var personalizacoes = await _personalizacaoService.GetPersonalizacoesParaRoboAsync(id);
        var viewModel = new RoboDetalhesPageViewModel
        {
            Robo = robo,
            Personalizacoes = personalizacoes
        };

        return View(viewModel);
    }
}