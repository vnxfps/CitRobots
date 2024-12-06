using System;
using System.Data;
using System.Collections.Generic;
using Newtonsoft.Json;
using CitRobots.Helpers;

namespace CitRobots.Services
{
    public class AdminStockService
    {
        private readonly DatabaseHelper _db;

        public AdminStockService()
        {
            _db = new DatabaseHelper();
        }

        public DataTable GetAllRobots()
        {
            string query = @"
                SELECT 
                    id as 'ID',
                    nome as 'Nome',
                    codigo_modelo as 'Código',
                    FORMAT(preco, 2, 'pt-BR') as 'Preço',
                    quantidade_estoque as 'Estoque',
                    quantidade_minima_estoque as 'Estoque Mínimo',
                    CASE WHEN ativo = 1 THEN 'Sim' ELSE 'Não' END as 'Ativo'
                FROM robos
                ORDER BY nome";

            return _db.ExecuteSelect(query);
        }

        public bool UpdateStock(int robotId, int newQuantity)
        {
            string query = @"
                UPDATE robos 
                SET quantidade_estoque = @Quantity,
                    ultima_atualizacao = NOW()
                WHERE id = @RobotId";

            var parameters = new Dictionary<string, object>
            {
                { "@RobotId", robotId },
                { "@Quantity", newQuantity }
            };

            try
            {
                var result = _db.ExecuteUpdate(query, parameters);
                if (result > 0)
                {
                    LogStockUpdate(robotId, newQuantity);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdatePrice(int robotId, decimal newPrice)
        {
            if (newPrice <= 0) return false;

            string query = @"
                UPDATE robos 
                SET preco = @Price,
                    ultima_atualizacao = NOW()
                WHERE id = @RobotId";

            var parameters = new Dictionary<string, object>
            {
                { "@RobotId", robotId },
                { "@Price", newPrice }
            };

            try
            {
                var result = _db.ExecuteUpdate(query, parameters);
                if (result > 0)
                {
                    LogPriceUpdate(robotId, newPrice);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void LogStockUpdate(int robotId, int newQuantity)
        {
            string query = @"
                INSERT INTO logs_sistema 
                (tipo, descricao, dados) 
                VALUES 
                ('ESTOQUE', 'Atualização de estoque', @Dados)";

            var dados = JsonConvert.SerializeObject(new
            {
                RoboId = robotId,
                NovaQuantidade = newQuantity,
                DataAlteracao = DateTime.Now
            });

            var parameters = new Dictionary<string, object>
            {
                { "@Dados", dados }
            };

            _db.ExecuteInsert(query, parameters);
        }

        private void LogPriceUpdate(int robotId, decimal newPrice)
        {
            string query = @"
                INSERT INTO logs_sistema 
                (tipo, descricao, dados) 
                VALUES 
                ('PRECO', 'Atualização de preço', @Dados)";

            var dados = JsonConvert.SerializeObject(new
            {
                RoboId = robotId,
                NovoPreco = newPrice,
                DataAlteracao = DateTime.Now
            });

            var parameters = new Dictionary<string, object>
            {
                { "@Dados", dados }
            };

            _db.ExecuteInsert(query, parameters);
        }

        public bool CheckMinimumStock(int robotId)
        {
            string query = @"
                SELECT quantidade_estoque, quantidade_minima_estoque 
                FROM robos 
                WHERE id = @RobotId";

            var parameters = new Dictionary<string, object>
            {
                { "@RobotId", robotId }
            };

            var result = _db.ExecuteSelect(query, parameters);
            if (result.Rows.Count > 0)
            {
                int currentStock = Convert.ToInt32(result.Rows[0]["quantidade_estoque"]);
                int minStock = Convert.ToInt32(result.Rows[0]["quantidade_minima_estoque"]);
                return currentStock <= minStock;
            }
            return false;
        }

        public bool ToggleRobotStatus(int robotId, bool ativo)
        {
            string query = "UPDATE robos SET ativo = @Ativo WHERE id = @RobotId";
            var parameters = new Dictionary<string, object>
            {
                { "@RobotId", robotId },
                { "@Ativo", ativo }
            };

            return _db.ExecuteUpdate(query, parameters) > 0;
        }
    }
}