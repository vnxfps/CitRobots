using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitRobots.Models
{
    public class PaymentModel
    {
        public int Id { get; set; }
        public int PedidoId { get; set; }
        public string TipoPagamento { get; set; }
        public string Status { get; set; }
        public decimal Valor { get; set; }
        public string DadosPagamento { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataProcessamento { get; set; }
        public DateTime? DataConfirmacao { get; set; }
    }
}