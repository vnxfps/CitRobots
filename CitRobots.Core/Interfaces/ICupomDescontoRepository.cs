public interface ICupomDescontoRepository : IBaseRepository<CupomDesconto>
{
    Task<CupomDesconto> GetByCodigoAsync(string codigo);
    Task<bool> IsValidAsync(string codigo);
    Task<decimal> CalcularDescontoAsync(string codigo, decimal valor);
}