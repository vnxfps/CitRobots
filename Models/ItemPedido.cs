namespace CitRobots.Models
{
    public class ItemPedido
    {
        public int RobotId { get; internal set; }
        public decimal Preco { get; internal set; }
        public object Personalizacoes { get; internal set; }
    }
}
