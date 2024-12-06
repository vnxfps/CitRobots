using System;
using System.Data;
using System.Collections.Generic;
using CitRobots.Models;
using Newtonsoft.Json;
using CitRobots.Helpers;

namespace CitRobots.Services
{
    public class OrderService
    {
        private readonly DatabaseHelper _db;

        public OrderService()
        {
            _db = new DatabaseHelper();
        }

        public int SalvarPedido(int clienteId, List<CartItemModel> items, decimal subtotal, decimal desconto, string cupom)
        {
            try
            {
                string numeroPedido = GerarNumeroPedido();
                string queryPedido = @"INSERT INTO pedidos 
                    (cliente_id, numero_pedido, status, data_pedido, subtotal, desconto, total) 
                    VALUES 
                    (@ClienteId, @NumeroPedido, 'PENDENTE', NOW(), @Subtotal, @Desconto, @Total);
                    SELECT LAST_INSERT_ID();";

                var paramsPedido = new Dictionary<string, object>
                {
                    { "@ClienteId", clienteId },
                    { "@NumeroPedido", numeroPedido },
                    { "@Subtotal", subtotal },
                    { "@Desconto", desconto },
                    { "@Total", subtotal - desconto }
                };

                // Use _db ao invés de DatabaseHelper
                int pedidoId = _db.ExecuteInsert(queryPedido, paramsPedido);

                foreach (var item in items)
                {
                    string queryItem = @"INSERT INTO itens_pedido 
                        (pedido_id, robo_id, quantidade, preco_unitario, subtotal, personalizacoes) 
                        VALUES 
                        (@PedidoId, @RoboId, 1, @PrecoUnitario, @Subtotal, @Personalizacoes)";

                    var paramsItem = new Dictionary<string, object>
                    {
                        { "@PedidoId", pedidoId },
                        { "@RoboId", item.Robot.Id },
                        { "@PrecoUnitario", item.PrecoUnitario },
                        { "@Subtotal", item.PrecoTotal },
                        { "@Personalizacoes", JsonConvert.SerializeObject(item.Customization) }
                    };

                    _db.ExecuteInsert(queryItem, paramsItem);
                }

                return pedidoId;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao salvar pedido: {ex.Message}");
            }
        }

        private string GerarNumeroPedido()
        {
            throw new NotImplementedException();
        }

        public DataTable BuscarPedidosCliente(int clienteId)
        {
            string query = @"SELECT p.*, COUNT(i.id) as quantidade_itens 
                           FROM pedidos p 
                           LEFT JOIN itens_pedido i ON p.id = i.pedido_id 
                           WHERE p.cliente_id = @ClienteId 
                           GROUP BY p.id 
                           ORDER BY p.data_pedido DESC";

            var parameters = new Dictionary<string, object>
            {
                { "@ClienteId", clienteId }
            };

            return _db.ExecuteSelect(query, parameters);
        }

        // Outros métodos usando _db ao invés de DatabaseHelper
        // ...
    }
}