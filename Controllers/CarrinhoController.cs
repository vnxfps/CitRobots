using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Text.Json;
using System;
using System.Collections.Generic;

public class CarrinhoController : Controller
{
    private readonly CitRobotsContext _context;

    public CarrinhoController(CitRobotsContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        // Carregar o carrinho da sessão
        var carrinho = HttpContext.Session.GetObject<Carrinho>("Carrinho") ?? new Carrinho();
        return View(carrinho);
    }

    [HttpPost]
    public IActionResult AplicarCupom(string codigoCupom)
    {
        var carrinho = HttpContext.Session.GetObject<Carrinho>("Carrinho");
        if (carrinho == null)
        {
            return RedirectToAction("Index");
        }

        // Verificar se o cupom é válido
        var cupom = _context.CuponsDesconto
            .FirstOrDefault(c => c.Codigo == codigoCupom && c.DataValidade >= DateTime.Now);

        if (cupom != null)
        {
            // Aplicar o cupom no carrinho
            carrinho.AplicarCupom(cupom);
            HttpContext.Session.SetObject("Carrinho", carrinho);  // Atualizar o carrinho na sessão
        }
        else
        {
            TempData["Error"] = "Cupom inválido ou expirado.";
        }

        return RedirectToAction("Index");
    }
}

internal class Item
{
    public string Nome { get; set; }
    public decimal Preco { get; set; }
    public int RobotId { get; internal set; }
    public IEnumerable<object> Personalizacoes { get; internal set; }
}

internal class Cupom
{
    public string Codigo { get; set; }
    public DateTime DataValidade { get; set; }
    public string TipoDesconto { get; set; }
    public decimal PercentualDesconto { get; set; }
    public decimal ValorDesconto { get; set; }
}

// Métodos de Extensão para a Sessão
public static class SessionExtensions
{
    public static void SetObject(this ISession session, string key, object value)
    {
        session.SetString(key, JsonSerializer.Serialize(value));
    }

    public static T GetObject<T>(this ISession session, string key)
    {
        var value = session.GetString(key);
        return value == null ? default(T) : JsonSerializer.Deserialize<T>(value);
    }
}
