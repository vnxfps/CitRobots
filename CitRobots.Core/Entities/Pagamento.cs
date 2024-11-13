public class Pagamento : BaseEntity
{
    public int PedidoId { get; set; }
    public int? CupomDescontoId { get; set; }
    public string FormaPagamento { get; set; }
    public DateTime DataPagamento { get; set; }
    public decimal ValorTotal { get; set; }
    public string Status { get; set; }
    public Pedido Pedido { get; set; }
    public CupomDesconto CupomDesconto { get; set; }
}