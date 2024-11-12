using CitRobots.Models;
using Microsoft.EntityFrameworkCore;

public class CupomService : ICupomService
{
    private readonly CitRobotsContext _context;

    public CupomService(CitRobotsContext context)
    {
        _context = context;
    }

    public async Task<CupomDesconto> ValidarCupomAsync(string codigo)
    {
        return await _context.CuponsDesconto
            .FirstOrDefaultAsync(c =>
                c.Codigo == codigo &&
                c.DataValidade >= DateTime.Now);
    }

    public async Task<decimal> AplicarDescontoAsync(string codigo, decimal valor)
    {
        var cupom = await ValidarCupomAsync(codigo);

        if (cupom == null)
            return valor;

        if (cupom.TipoDesconto == "Porcentagem")
            return valor * (1 - cupom.ValorDesconto);

        return valor - cupom.ValorDesconto;
    }
}