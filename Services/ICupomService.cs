using CitRobots.Models;

public interface ICupomService
{
    Task<CupomDesconto> ValidarCupomAsync(string codigo);
    Task<decimal> AplicarDescontoAsync(string codigo, decimal valor);
}