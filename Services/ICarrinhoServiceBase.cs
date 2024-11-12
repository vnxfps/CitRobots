namespace CitRobots.Models
{
    public class Carrinho
    {
        public List<CarrinhoItem> Items { get; set; } = new List<CarrinhoItem>();
        public decimal Total { get; set; }
    }
}
