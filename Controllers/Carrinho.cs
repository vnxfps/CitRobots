using CitRobots.Models;

internal class Carrinho
{
    public List<Item> Items { get; set; } = new List<Item>();
    public decimal Total { get; set; }

    public void AplicarCupom(Cupom cupom)
    {
        if (cupom.TipoDesconto == "Percentual")
        {
            Total -= Total * cupom.PercentualDesconto / 100;
        }
        else
        {
            Total -= cupom.ValorDesconto;
        }
    }

    internal void AplicarCupom(CupomDesconto cupom)
    {
        throw new NotImplementedException();
    }
}
