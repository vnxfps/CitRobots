public class ItemPedido : BaseEntity
{
    public int PedidoId { get; set; }
    public int RoboId { get; set; }
    public int PersonalizacaoId { get; set; }
    public decimal PrecoUnitario { get; set; }
    public Pedido Pedido { get; set; }
    public Robo Robo { get; set; }
    public Personalizacao Personalizacao { get; set; }
}