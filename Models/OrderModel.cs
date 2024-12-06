using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitRobots.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public string NumeroPedido { get; set; }
        public string Status { get; set; }
        public DateTime DataPedido { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Desconto { get; set; }
        public decimal Total { get; set; }
        public List<CartItemModel> Itens { get; set; }
    }
}
