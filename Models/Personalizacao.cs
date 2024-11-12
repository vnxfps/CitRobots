namespace CitRobots.Models
{
    public class Personalizacao
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public string Descricao { get; set; }
        public string Tipo { get; set; }
        public virtual ICollection<RobotPersonalizacao> Robots { get; set; }
    }
}