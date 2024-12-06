using CitRobots.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitRobots.Helpers
{
    public class RobotDataAdapter
    {
        private readonly DatabaseHelper _db;

        public RobotDataAdapter() => _db = new DatabaseHelper();

        public List<RobotModel> GetAllRobots()
        {
            const string query = @"
            SELECT r.*, c.nome as categoria_nome 
            FROM robos r 
            JOIN categorias_robos c ON r.categoria_id = c.id 
            WHERE r.ativo = 1";

            var dt = _db.ExecuteSelect(query);
            return ConvertToRobotList(dt);
        }

        public RobotModel GetRobotById(int id)
        {
            string query = "SELECT * FROM robos WHERE id = @Id AND ativo = 1";
            var dt = _db.ExecuteSelect(query, new Dictionary<string, object> { { "@Id", id } });
            return ConvertToRobotList(dt).FirstOrDefault();
        }

        private List<RobotModel> ConvertToRobotList(DataTable dt) =>
            dt.AsEnumerable().Select(row => new RobotModel
            {
                Id = Convert.ToInt32(row["id"]),
                CategoriaId = Convert.ToInt32(row["categoria_id"]),
                Nome = row["nome"].ToString(),
                Codigo = row["codigo_modelo"].ToString(),
                CodigoModelo = row["codigo_modelo"].ToString(),
                Descricao = row["descricao"].ToString(),
                Preco = Convert.ToDecimal(row["preco"]),
                QuantidadeEstoque = Convert.ToInt32(row["quantidade_estoque"]),
                ImagemUrl = row["imagem_url"].ToString(),
                Especificacoes = row["especificacoes"].ToString(),
                Ativo = Convert.ToBoolean(row["ativo"])
            }).ToList();
    }
}
