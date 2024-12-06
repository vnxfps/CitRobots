using System;
using System.Data;
using System.Collections.Generic;
using CitRobots.Models;
using CitRobots.Helpers;
using Newtonsoft.Json;

namespace CitRobots.Services
{
    public class OrderService
    {
        private readonly DatabaseHelper _db;
        private readonly StockService _stockService;

        public OrderService()
        {
            _db = new DatabaseHelper();
            _stockService = new StockService();
        }

        public int SalvarPedido(int clienteId, List<CartItemModel> items, decimal subtotal, decimal desconto)
        {
            string numeroPedido = GerarNumeroPedido();

            try
            {
                string queryPedido = @"INSERT INTO pedidos 
                (cliente_id, numero_pedido, status, data_pedido, subtotal, desconto, total) 
                VALUES (@ClienteId, @NumeroPedido, 'PENDENTE', NOW(), @Subtotal, @Desconto, @Total)";

                var paramsPedido = new Dictionary<string, object>
            {
                { "@ClienteId", clienteId },
                { "@NumeroPedido", numeroPedido },
                { "@Subtotal", subtotal },
                { "@Desconto", desconto },
                { "@Total", subtotal - desconto }
            };

                int pedidoId = _db.ExecuteInsert(queryPedido, paramsPedido);

                foreach (var item in items)
                {
                    if (!_stockService.DiminuirEstoque(item.Robot.Id, item.Quantidade))
                        throw new Exception($"Estoque insuficiente para {item.Robot.Nome}");

                    SalvarItemPedido(pedidoId, item);
                }

                return pedidoId;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao salvar pedido: {ex.Message}");
            }
        }

        private void SalvarItemPedido(int pedidoId, CartItemModel item)
        {
            string query = @"INSERT INTO itens_pedido 
            (pedido_id, robo_id, quantidade, preco_unitario, subtotal, personalizacoes) 
            VALUES (@PedidoId, @RoboId, @Quantidade, @PrecoUnit, @Subtotal, @Personalizacoes)";

            var parameters = new Dictionary<string, object>
        {
            { "@PedidoId", pedidoId },
            { "@RoboId", item.Robot.Id },
            { "@Quantidade", item.Quantidade },
            { "@PrecoUnit", item.PrecoUnitario },
            { "@Subtotal", item.PrecoTotal },
            { "@Personalizacoes", JsonConvert.SerializeObject(item.Customization) }
        };

            _db.ExecuteInsert(query, parameters);
        }

        private string GerarNumeroPedido() =>
            DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(1000, 9999).ToString();
    }
}