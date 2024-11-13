using CitRobots.Models;

public class CupomDesconto : BaseEntity
{
    public string Codigo { get; set; }
    public string Descricao { get; set; }
    public decimal ValorDesconto { get; set; }
    public DateTime DataValidade { get; set; }
    public string TipoDesconto { get; set; }
    public ICollection<Pagamento> Pagamentos { get; set; }
}