using CitRobots.Models;

public class Pedido : BaseEntity
{
    public int ClienteId { get; set; }
    public DateTime DataPedido { get; set; }
    public decimal Total { get; set; }
    public string Status { get; set; }
    public Cliente Cliente { get; set; }
    public ICollection<ItemPedido> ItensPedido { get; set; }
    public Pagamento Pagamento { get; set; }
}