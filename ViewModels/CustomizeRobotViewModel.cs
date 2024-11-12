using CitRobots.Models;

public class CustomizeRobotViewModel
{
    public Robot Robot { get; set; }
    public List<Personalizacao> Personalizacoes { get; set; }
    public List<int> PersonalizacaoSelecionadaIds { get; set; }
}