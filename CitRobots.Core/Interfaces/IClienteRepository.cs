public interface IClienteRepository : IBaseRepository<Cliente>
{
    Task<Cliente> GetByUsuarioIdAsync(int usuarioId);
    Task<bool> CpfExistsAsync(string cpf);
    Task<IEnumerable<Pedido>> GetPedidosAsync(int clienteId);
}