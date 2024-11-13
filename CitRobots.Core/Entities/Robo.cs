using CitRobots.Models;

public class Robo : BaseEntity
{
    public string Nome { get; set; }
    public decimal Preco { get; set; }
    public int Quantidade { get; set; }
    public string Slogan { get; set; }
    public string Descricao { get; set; }
    public string ImagemUrl { get; set; }
    public ICollection<ItemPedido> ItensPedido { get; set; }
}