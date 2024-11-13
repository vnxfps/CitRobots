using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
public class CarrinhoController : Controller
{
    private readonly ICarrinhoService _carrinhoService;
    private readonly ICupomDescontoService _cupomService;

    public CarrinhoController(
        ICarrinhoService carrinhoService,
        ICupomDescontoService cupomService)
    {
        _carrinhoService = carrinhoService;
        _cupomService = cupomService;
    }

    public async Task<IActionResult> Index()
    {
        var carrinho = await _carrinhoService.ObterCarrinhoAsync();
        return View(carrinho);
    }

    [HttpPost]
    public async Task<IActionResult> AplicarCupom(string codigo)
    {
        var resultado = await _cupomService.AplicarCupomAsync(codigo);
        return Json(new { sucesso = resultado.Sucesso, mensagem = resultado.Mensagem });
    }
}