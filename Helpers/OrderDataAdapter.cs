using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitRobots.Helpers
{
    public class OrderDataAdapter
    {
        private readonly DatabaseHelper _db;

        public OrderDataAdapter() => _db = new DatabaseHelper();

        public DataTable GetOrdersByClient(int clientId)
        {
            string query = @"
            SELECT 
                p.id, p.numero_pedido, p.status, p.data_pedido,
                p.subtotal, p.desconto, p.total,
                COUNT(i.id) as quantidade_itens
            FROM pedidos p 
            LEFT JOIN itens_pedido i ON p.id = i.pedido_id
            WHERE p.cliente_id = @ClientId
            GROUP BY p.id
            ORDER BY p.data_pedido DESC";

            return _db.ExecuteSelect(query, new Dictionary<string, object> { { "@ClientId", clientId } });
        }

        public DataTable GetOrderDetails(int orderId)
        {
            string query = @"
            SELECT 
                i.*, r.nome as robo_nome, r.codigo_modelo
            FROM itens_pedido i
            JOIN robos r ON i.robo_id = r.id
            WHERE i.pedido_id = @OrderId";

            return _db.ExecuteSelect(query, new Dictionary<string, object> { { "@OrderId", orderId } });
        }
    }
}
