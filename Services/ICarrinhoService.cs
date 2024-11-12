using System.Collections.Generic;
using System.Threading.Tasks;

public interface ICarrinhoService
{
    Task<CarrinhoViewModel> GetCarrinhoAsync(string userId);
    Task AddItemAsync(string userId, int robotId, List<int> personalizacaoIds);
    Task RemoveItemAsync(string userId, int itemId);
    Task ClearCarrinhoAsync(string userId);
    Task ProcessarPedidoAsync(string userId, EnderecoEntrega endereco, string formaPagamento);
    Task ProcessarPedidoAsync(string? name, EnderecoEntrega enderecoEntrega, object formaPagamento);
}
