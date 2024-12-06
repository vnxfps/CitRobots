using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitRobots.Models
{
    public class RobotCustomizationModel
    {
        public string Cor { get; set; }
        public string Voz { get; set; }
        public string Tamanho { get; set; }
        public string Peso { get; set; }
        public bool TemReplay { get; set; }
        public bool TemMonitoramento { get; set; }

        public decimal CalcularPrecoAdicionais()
        {
            decimal total = 0;
            if (TemReplay) total += 10000;
            if (TemMonitoramento) total += 10000;
            return total;
        }
    }
}