using CitRobots.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitRobots.Services
{
    public class StockService
    {
        private readonly DatabaseHelper _db;

        public StockService() => _db = new DatabaseHelper();

        public bool VerificarEstoque(int robotId, int quantidade)
        {
            string query = "SELECT quantidade_estoque FROM robos WHERE id = @RobotId AND ativo = 1";
            var parameters = new Dictionary<string, object> { { "@RobotId", robotId } };
            var result = _db.ExecuteSelect(query, parameters);

            return result.Rows.Count > 0 &&
                   Convert.ToInt32(result.Rows[0]["quantidade_estoque"]) >= quantidade;
        }

        public bool DiminuirEstoque(int robotId, int quantidade)
        {
            if (!VerificarEstoque(robotId, quantidade)) return false;

            string query = "UPDATE robos SET quantidade_estoque = quantidade_estoque - @Quantidade WHERE id = @RobotId";
            var parameters = new Dictionary<string, object>
        {
            { "@RobotId", robotId },
            { "@Quantidade", quantidade }
        };

            return _db.ExecuteUpdate(query, parameters) > 0;
        }
    }
}
