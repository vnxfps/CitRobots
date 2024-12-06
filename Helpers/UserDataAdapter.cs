using CitRobots.Models;
using CitRobots.Services;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitRobots.Helpers
{
    public class UserDataAdapter
    {
        private readonly DatabaseHelper _db;
        private readonly AuthenticationService _auth;

        public UserDataAdapter()
        {
            _db = new DatabaseHelper();
            _auth = new AuthenticationService();
        }

        public int CreateUser(UserModel user)
        {
            MySqlConnection connection = _db.GetConnection();
            MySqlTransaction transaction = null;

            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();

                string userQuery = @"
                INSERT INTO usuarios (login, senha_hash, email, tipo_usuario) 
                VALUES (@Email, @Password, @Email, 'CLIENTE');
                SELECT LAST_INSERT_ID();";

                var userParams = new Dictionary<string, object>
            {
                { "@Email", user.Email },
                { "@Password", _auth.HashPassword(user.Senha) }
            };

                int userId = _db.ExecuteInsert(userQuery, userParams);

                if (userId > 0)
                {
                    string clienteQuery = @"
                    INSERT INTO clientes 
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
                    { "@DataNascimento", DateTime.Parse(user.DataNascimento) },
                    { "@Telefone", user.Telefone },
                    { "@Endereco", user.Endereco }
                };

                    _db.ExecuteInsert(clienteQuery, clienteParams);
                }

                transaction.Commit();
                return userId;
            }
            catch
            {
                if (transaction != null)
                    transaction.Rollback();
                throw;
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }

        public bool UpdateUser(UserModel user)
        {
            string query = @"
            UPDATE clientes 
            SET nome = @Nome, 
                sobrenome = @Sobrenome, 
                telefone = @Telefone,
                endereco_entrega = @Endereco
            WHERE usuario_id = @UserId";

            var parameters = new Dictionary<string, object>
        {
            { "@UserId", user.Id },
            { "@Nome", user.Nome },
            { "@Sobrenome", user.Sobrenome },
            { "@Telefone", user.Telefone },
            { "@Endereco", user.Endereco }
        };

            return _db.ExecuteUpdate(query, parameters) > 0;
        }
    }
}
