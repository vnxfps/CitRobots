using System;
using System.Data;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using CitRobots.Helpers;
using CitRobots.Models;
using Newtonsoft.Json;

namespace CitRobots.Services
{
    public class DatabaseService
    {
        private readonly DatabaseHelper _dbHelper;

        public DatabaseService()
        {
            _dbHelper = new DatabaseHelper();
        }

        public DataTable ValidateLogin(string email, string password)
        {
            string query = @"SELECT u.*, c.id as cliente_id, c.nome, c.sobrenome 
                           FROM usuarios u 
                           LEFT JOIN clientes c ON u.id = c.usuario_id 
                           WHERE u.email = @Email AND u.senha_hash = @Password AND u.ativo = 1";

            var parameters = new Dictionary<string, object>
            {
                { "@Email", email },
                { "@Password", HashPassword(password) }
            };

            return _dbHelper.ExecuteSelect(query, parameters);
        }

        public int RegisterUser(UserModel user)
        {
            string userQuery = @"INSERT INTO usuarios (login, senha_hash, email, tipo_usuario, data_criacao, ativo) 
                               VALUES (@Email, @Password, @Email, 'CLIENTE', NOW(), 1);
                               SELECT LAST_INSERT_ID();";

            var userParams = new Dictionary<string, object>
            {
                { "@Email", user.Email },
                { "@Password", HashPassword(user.Senha) }
            };

            int userId = _dbHelper.ExecuteInsert(userQuery, userParams);

            if (userId > 0)
            {
                string clienteQuery = @"INSERT INTO clientes 
                    (usuario_id, nome, sobrenome, email, cpf, data_nascimento, telefone, endereco_entrega) 
                    VALUES 
                    (@UserId, @Nome, @Sobrenome, @Email, @CPF, @DataNascimento, @Telefone, @Endereco)";

                var clienteParams = new Dictionary<string, object>
                {
                    { "@UserId", userId },
                    { "@Nome", user.Nome },
                    { "@Sobrenome", user.Sobrenome },
                    { "@Email", user.Email },
                    { "@CPF", user.CPF },
                    { "@DataNascimento", user.DataNascimento },
                    { "@Telefone", user.Telefone },
                    { "@Endereco", JsonConvert.SerializeObject(user.Endereco) }
                };

                _dbHelper.ExecuteInsert(clienteQuery, clienteParams);
            }

            return userId;
        }

        public UserModel GetUserData(int userId)
        {
            string query = @"SELECT u.*, c.* 
                           FROM usuarios u 
                           JOIN clientes c ON u.id = c.usuario_id 
                           WHERE u.id = @UserId";

            var parameters = new Dictionary<string, object> { { "@UserId", userId } };
            var dt = _dbHelper.ExecuteSelect(query, parameters);

            if (dt.Rows.Count > 0)
            {
                var row = dt.Rows[0];
                return new UserModel
                {
                    Id = Convert.ToInt32(row["id"]),
                    Nome = row["nome"].ToString(),
                    Sobrenome = row["sobrenome"].ToString(),
                    Email = row["email"].ToString(),
                    CPF = row["cpf"].ToString(),
                    Telefone = row["telefone"].ToString(),
                    Endereco = row["endereco_entrega"].ToString()
                };
            }
            return null;
        }

        public void AtualizarDadosUsuario(UserModel user)
        {
            string query = @"UPDATE clientes 
                           SET nome = @Nome, 
                               sobrenome = @Sobrenome,
                               email = @Email,
                               telefone = @Telefone,
                               endereco_entrega = @Endereco
                           WHERE usuario_id = @UserId";

            var parameters = new Dictionary<string, object>
            {
                { "@UserId", user.Id },
                { "@Nome", user.Nome },
                { "@Sobrenome", user.Sobrenome },
                { "@Email", user.Email },
                { "@Telefone", user.Telefone },
                { "@Endereco", JsonConvert.SerializeObject(user.Endereco) }
            };

            _dbHelper.ExecuteUpdate(query, parameters);
        }

        public int SalvarPedido(List<CartItemModel> items, int clienteId, decimal total, string cupom)
        {
            try
            {
                string numeroPedido = GerarNumeroPedido();
                string queryPedido = @"INSERT INTO pedidos 
                    (cliente_id, numero_pedido, status, data_pedido, subtotal, total) 
                    VALUES 
                    (@ClienteId, @NumeroPedido, 'PENDENTE', NOW(), @Subtotal, @Total)";

                var paramsPedido = new Dictionary<string, object>
                {
                    { "@ClienteId", clienteId },
                    { "@NumeroPedido", numeroPedido },
                    { "@Subtotal", total },
                    { "@Total", total }
                };

                int pedidoId = _dbHelper.ExecuteInsert(queryPedido, paramsPedido);

                foreach (var item in items)
                {
                    string queryItem = @"INSERT INTO itens_pedido 
                        (pedido_id, robo_id, quantidade, preco_unitario, subtotal, cor_id, voz_id, dimensao_id) 
                        VALUES 
                        (@PedidoId, @RoboId, 1, @PrecoUnitario, @Subtotal, @CorId, @VozId, @DimensaoId)";

                    var paramsItem = new Dictionary<string, object>
                    {
                        { "@PedidoId", pedidoId },
                        { "@RoboId", item.Robot.Id },
                        { "@PrecoUnitario", item.PrecoUnitario },
                        { "@Subtotal", item.PrecoTotal },
                        { "@CorId", GetCorId(item.Customization.Cor) },
                        { "@VozId", GetVozId(item.Customization.Voz) },
                        { "@DimensaoId", GetDimensaoId(item.Customization.Tamanho, item.Customization.Peso) }
                    };

                    _dbHelper.ExecuteInsert(queryItem, paramsItem);
                }

                return pedidoId;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao salvar pedido: {ex.Message}");
            }
        }

        public DataTable GetPedidosCliente(int clienteId)
        {
            string query = @"SELECT p.*, COUNT(i.id) as quantidade_itens 
                           FROM pedidos p 
                           LEFT JOIN itens_pedido i ON p.id = i.pedido_id 
                           WHERE p.cliente_id = @ClienteId 
                           GROUP BY p.id 
                           ORDER BY p.data_pedido DESC";

            var parameters = new Dictionary<string, object> { { "@ClienteId", clienteId } };
            return _dbHelper.ExecuteSelect(query, parameters);
        }

        private string GerarNumeroPedido()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(1000, 9999).ToString();
        }

        private int GetCorId(string cor)
        {
            string query = "SELECT id FROM opcoes_cores WHERE nome = @Nome";
            var parameters = new Dictionary<string, object> { { "@Nome", cor } };
            var result = _dbHelper.ExecuteSelect(query, parameters);
            return result.Rows.Count > 0 ? Convert.ToInt32(result.Rows[0]["id"]) : 1;
        }

        private int GetVozId(string voz)
        {
            string query = "SELECT id FROM opcoes_vozes WHERE nome = @Nome";
            var parameters = new Dictionary<string, object> { { "@Nome", voz } };
            var result = _dbHelper.ExecuteSelect(query, parameters);
            return result.Rows.Count > 0 ? Convert.ToInt32(result.Rows[0]["id"]) : 1;
        }

        private int GetDimensaoId(string altura, string peso)
        {
            string query = "SELECT id FROM opcoes_dimensoes WHERE altura = @Altura AND peso = @Peso";
            var parameters = new Dictionary<string, object>
            {
                { "@Altura", Convert.ToDecimal(altura.Replace("m", "")) },
                { "@Peso", Convert.ToInt32(peso.Replace("kg", "")) }
            };
            var result = _dbHelper.ExecuteSelect(query, parameters);
            return result.Rows.Count > 0 ? Convert.ToInt32(result.Rows[0]["id"]) : 1;
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}