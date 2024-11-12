
namespace CitRobots.Models
{
    public class CupomDesconto
    {
        public string TipoDesconto { get; internal set; }
        public DateTime DataValidade { get; internal set; }
        public string Codigo { get; internal set; }
        public int ValorDesconto { get; internal set; }
    }
}
