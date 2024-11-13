using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
public class PedidoController : Controller
{
    private readonly CitRobotsContext _context;
    private readonly ICarrinhoService _carrinhoService;

    public PedidoController(
        CitRobotsContext context,
        ICarrinhoService carrinhoService)
    {
        _context = context;
        _carrinhoService = carrinhoService;
    }

    public async Task<IActionResult> Checkout()
    {
        var carrinho = await _carrinhoService.GetCarrinhoAsync(userId: User.Identity.Name);
        var viewModel = new CheckoutViewModel
        {
            Carrinho = carrinho,
            EnderecoEntrega = new EnderecoEntrega()
        };

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> ProcessPayment(CheckoutViewModel model)
    {
        if (ModelState.IsValid)
        {
            await _carrinhoService.ProcessarPedidoAsync(
                userId: User.Identity.Name, model.EnderecoEntrega, model.FormaPagamento);

            return RedirectToAction("Confirmation");
        }

        return View("Checkout", model);
    }
}

public class EnderecoEntrega
{
    public EnderecoEntrega()
    {
    }
}