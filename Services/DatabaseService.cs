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
                    { "@Endereco", user.Endereco }
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

            var parameters = new Dictionary<string, object>
        {
            { "@UserId", userId }
        };

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
                };
            }
            return null;
        }

        public void UpdateUserData(UserModel user)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@Id", user.Id },
                { "@Nome", user.Nome },
                { "@Email", user.Email }
            };

            string query = "UPDATE usuarios SET ... WHERE id = @Id";
            _dbHelper.ExecuteUpdate(query, parameters);
        }



        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        internal int SalvarPedido(List<CartItemModel> items, int clienteId, decimal total, string text)
        {
            throw new NotImplementedException();
        }

        internal object GetPedidosCliente(int clienteId)
        {
            throw new NotImplementedException();
        }

        internal void AtualizarDadosUsuario(UserModel currentUser)
        {
            throw new NotImplementedException();
        }
    }
}