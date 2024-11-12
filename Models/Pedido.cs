using CitRobots.Models;

public class Pedido
{
    public int ClienteId { get; set; }
    public DateTime DataPedido { get; set; }
    public EnderecoEntrega EnderecoEntrega { get; set; }
    public string FormaPagamento { get; set; }
    public decimal Total { get; set; }

    public List<ItemPedido> Items { get; set; } = new List<ItemPedido>();
}
