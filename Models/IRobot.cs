
namespace CitRobots.Models
{
    public interface IRobot
    {
        int GetId();
        void SetId(int value);

        int Id { get; set; }
        int Id { get; set; }
        string ImagemUrl { get; set; }
        string ImagemUrl { get; set; }
        string ImagemUrl { get; set; }
        string Nome { get; set; }
        string Nome { get; set; }
        string Nome { get; set; }
        ICollection<RobotPersonalizacao> Personalizacoes { get; set; }
        decimal Preco { get; set; }
        decimal PrecoBase { get; set; }
        decimal PrecoBase { get; set; }
        int Quantidade { get; set; }
        string Slogan { get; set; }
    }
}