using CitRobots.Models;

public class Personalizacao : BaseEntity
{
    public string Nome { get; set; }
    public decimal Preco { get; set; }
    public string Descricao { get; set; }
    public string Tipo { get; set; }
    public ICollection<RoboPersonalizacao> RoboPersonalizacoes { get; set; }
}