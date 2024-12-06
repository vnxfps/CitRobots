using CitRobots.Helpers;
using CitRobots.Models;
using System;
using System.Collections.Generic;

namespace CitRobots.Services
{
    public class PaymentService
    {
        private readonly DatabaseHelper _db;

        public PaymentService()
        {
            _db = new DatabaseHelper();
        }

        public bool ProcessarPagamento(int pedidoId, string tipoPagamento, decimal valor, string dadosPagamento)
        {
            string query = @"INSERT INTO pagamentos 
            (pedido_id, tipo_pagamento, status, valor, dados_pagamento, data_criacao) 
            VALUES (@PedidoId, @Tipo, 'PENDENTE', @Valor, @Dados, NOW())";

            var parameters = new Dictionary<string, object>
        {
            { "@PedidoId", pedidoId },
            { "@Tipo", tipoPagamento },
            { "@Valor", valor },
            { "@Dados", dadosPagamento }
        };

            try
            {
                return _db.ExecuteInsert(query, parameters) > 0;
            }
            catch
            {
                return false;
            }
        }

        public bool AtualizarStatusPagamento(int pedidoId, string status)
        {
            string query = @"UPDATE pagamentos 
            SET status = @Status, data_processamento = NOW() 
            WHERE pedido_id = @PedidoId";

            var parameters = new Dictionary<string, object>
        {
            { "@PedidoId", pedidoId },
            { "@Status", status }
        };

            return _db.ExecuteUpdate(query, parameters) > 0;
        }
    }
}