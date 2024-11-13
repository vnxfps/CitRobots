public class Cliente : BaseEntity
{
    public int UsuarioId { get; set; }
    public string Nome { get; set; }
    public string Sobrenome { get; set; }
    public string Email { get; set; }
    public string CPF { get; set; }
    public DateTime DataNascimento { get; set; }
    public string Endereco { get; set; }
    public Usuario Usuario { get; set; }
    public ICollection<Pedido> Pedidos { get; set; }
}