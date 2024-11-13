public interface IPedidoRepository : IBaseRepository<Pedido>
{
    Task<IEnumerable<Pedido>> GetByClienteIdAsync(int clienteId);
    Task<Pedido> GetPedidoCompletoAsync(int pedidoId);
    Task<bool> UpdateStatusAsync(int pedidoId, string status);
}