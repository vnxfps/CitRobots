using System.ComponentModel.DataAnnotations;

namespace CitRobots.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        public string Sobrenome { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string CPF { get; set; }
        [Required]
        public DateTime DataNascimento { get; set; }
        [Required]
        public string Endereco { get; set; }
        public virtual ICollection<Pedido> Pedidos { get; set; }
    }
}