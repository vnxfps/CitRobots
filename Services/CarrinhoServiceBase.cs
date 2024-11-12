using CitRobots.Models;
using Microsoft.EntityFrameworkCore;

public class CarrinhoServiceBase
{
    private readonly CitRobotsContext _context;

    public CarrinhoServiceBase(CitRobotsContext context)
    {
        _context = context;
    }

    public async Task ProcessarPedidoAsync(string userId, EnderecoEntrega endereco, string formaPagamento)
    {
        var cliente = await _context.Clientes
            .FirstOrDefaultAsync(c => c.Usuario.Login == userId);

        if (cliente == null)
        {
            throw new ArgumentException($"Client with user ID {userId} not found.");
        }

        var carrinho = await GetCarrinhoAsync(userId);
        var pedido = new Pedido
        {
            ClienteId = cliente.Id,
            DataPedido = DateTime.Now,
            EnderecoEntrega = endereco,
            FormaPagamento = formaPagamento,
            Total = carrinho.Total
        };

        foreach (var item in carrinho.Items)
        {
            var itemPedido = new ItemPedido
            {
                RobotId = item.RobotId,
                Preco = item.Preco
            };

            foreach (var personalizacao in item.Personalizacoes)
            {
                itemPedido.Personalizacoes.Add(personalizacao);
            }

            pedido.Items.Add(itemPedido);
        }

        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync();
        await ClearCarrinhoAsync(userId);
    }

    private async Task GetCarrinhoAsync(string userId)
    {
        throw new NotImplementedException();
    }

    private async Task ClearCarrinhoAsync(string userId)
    {
        throw new NotImplementedException();
    }
}
