public interface IPagamentoRepository : IBaseRepository<Pagamento>
{
    Task<Pagamento> GetByPedidoIdAsync(int pedidoId);
    Task<bool> ProcessarPagamentoAsync(Pagamento pagamento);
    Task<bool> UpdateStatusAsync(int pagamentoId, string status);
}