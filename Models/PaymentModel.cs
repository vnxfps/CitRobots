using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitRobots.Models
{
    public class PaymentModel
    {
        public int PedidoId { get; set; }
        public string TipoPagamento { get; set; }
        public decimal Valor { get; set; }
        public string Status { get; set; }
        public string DadosPagamento { get; set; }
    }
}