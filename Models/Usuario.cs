using System.ComponentModel.DataAnnotations;

namespace CitRobots.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        public string Senha { get; set; }
        public virtual Cliente Cliente { get; set; }
        public string Email { get; internal set; }
    }
}