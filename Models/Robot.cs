using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CitRobots.Models
{
    public class Robot
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(40)]
        public string Nome { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Preco { get; set; }

        [Required]
        public int Quantidade { get; set; }

        public string Slogan { get; set; }
        public string ImagemUrl { get; set; }
        public virtual ICollection<RobotPersonalizacao> Personalizacoes { get; set; }
    }
}