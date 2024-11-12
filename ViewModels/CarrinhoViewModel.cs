using CitRobots.Models;

public class CarrinhoViewModel
{
    public List<CarrinhoItem> Items { get; set; }
    public decimal Total { get; set; }
    public CupomDesconto CupomDesconto { get; set; }
}

public class CarrinhoItem
{
    private List<Personalizacao> personalizacoes;

    public int RobotId { get; internal set; }
    public Robot? Robot { get; internal set; } 
    public decimal Preco { get; internal set; }
    public List<Personalizacao> Personalizacoes { get => personalizacoes; internal set => personalizacoes = value; }
}

public class Robot
{
    internal object Personalizacoes;

    public int Id { get; set; }
    public string Nome { get; set; }
    public string ImagemUrl { get; set; }
}


public class Personalizacao
{
    public string Nome { get; set; }
}
