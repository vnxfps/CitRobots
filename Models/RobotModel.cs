using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitRobots.Models
{
    public class RobotModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string ImagemUrl { get; set; }
        public decimal Preco { get; set; }
        public string Slogan { get; set; }
        public string Codigo { get; set; }
        public RobotCustomizationModel Customization { get; set; }

        public decimal CalcularPrecoTotal()
        {
            return Preco + (Customization?.CalcularPrecoAdicionais() ?? 0);
        }
    }
}