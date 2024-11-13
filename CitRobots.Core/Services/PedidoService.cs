public class PedidoService : BaseService<Pedido>, IPedidoService
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IRoboRepository _roboRepository;
    private readonly IClienteRepository _clienteRepository;

    public PedidoService(
        IPedidoRepository pedidoRepository,
        IRoboRepository roboRepository,
        IClienteRepository clienteRepository,
        ILogger<PedidoService> logger) : base(pedidoRepository, logger)
    {
        _pedidoRepository = pedidoRepository;
        _roboRepository = roboRepository;
        _clienteRepository = clienteRepository;
    }

    public async Task<Pedido> CreatePedidoAsync(PedidoDTO pedidoDTO)
    {
        try
        {
            var cliente = await _clienteRepository.GetByIdAsync(pedidoDTO.ClienteId);
            if (cliente == null)
                throw new NotFoundException("Cliente não encontrado");

            var pedido = new Pedido
            {
                ClienteId = pedidoDTO.ClienteId,
                DataPedido = DateTime.UtcNow,
                Status = "Pendente",
                Total = await CalcularTotalPedido(pedidoDTO.Itens)
            };

            foreach (var item in pedidoDTO.Itens)
            {
                var robo = await _roboRepository.GetByIdAsync(item.RoboId);
                if (robo == null || robo.Quantidade < 1)
                    throw new BusinessException($"Robô {item.RoboId} não disponível");

                await _roboRepository.UpdateEstoqueAsync(item.RoboId, robo.Quantidade - 1);
            }

            return await _pedidoRepository.AddAsync(pedido);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating order");
            throw;
        }
    }

    private async Task<decimal> CalcularTotalPedido(IEnumerable<ItemPedidoDTO> itens)
    {
        decimal total = 0;
        foreach (var item in itens)
        {
            var robo = await _roboRepository.GetByIdAsync(item.RoboId);
            total += robo.Preco;
        }
        return total;
    }
}