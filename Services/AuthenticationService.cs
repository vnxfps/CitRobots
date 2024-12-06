using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using CitRobots.Helpers;
using CitRobots.Models;

namespace CitRobots.Services
{
    public class AuthenticationService
    {
        private readonly DatabaseHelper _db;

        public AuthenticationService()
        {
            _db = new DatabaseHelper();
        }

        public bool ValidateLogin(string email, string password, out UserModel user)
        {
            user = null;
            try
            {
                string query = @"
                    SELECT u.*, c.id as cliente_id, c.nome, c.sobrenome, c.cpf, 
                           c.telefone, c.endereco_entrega 
                    FROM usuarios u 
                    LEFT JOIN clientes c ON u.id = c.usuario_id 
                    WHERE u.email = @Email AND u.senha_hash = @Password AND u.ativo = 1";

                var parameters = new Dictionary<string, object>
                {
                    { "@Email", email },
                    { "@Password", HashPassword(password) }
                };

                var result = _db.ExecuteSelect(query, parameters);
                if (result != null && result.Rows.Count > 0)
                {
                    var row = result.Rows[0];
                    user = new UserModel
                    {
                        Id = Convert.ToInt32(row["id"]),
                        Email = row["email"].ToString(),
                        Nome = row["nome"].ToString(),
                        Sobrenome = row["sobrenome"].ToString(),
                        CPF = row["cpf"].ToString(),
                        Telefone = row["telefone"].ToString(),
                        TipoUsuario = row["tipo_usuario"].ToString(),
                        Endereco = row["endereco_entrega"].ToString()
                    };

                    UpdateLastAccess(user.Id);
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao validar login: {ex.Message}");
            }
            return false;
        }

        private void UpdateLastAccess(int userId)
        {
            string query = "UPDATE usuarios SET ultimo_acesso = NOW() WHERE id = @UserId";
            var parameters = new Dictionary<string, object> { { "@UserId", userId } };
            _db.ExecuteUpdate(query, parameters);
        }

        public bool UpdatePassword(int userId, string newPassword)
        {
            string query = "UPDATE usuarios SET senha_hash = @Password WHERE id = @UserId";
            var parameters = new Dictionary<string, object>
            {
                { "@UserId", userId },
                { "@Password", HashPassword(newPassword) }
            };
            return _db.ExecuteUpdate(query, parameters) > 0;
        }

        public string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                return hash;
            }
        }

        public bool EmailExists(string email)
        {
            string query = "SELECT COUNT(*) FROM usuarios WHERE email = @Email";
            var parameters = new Dictionary<string, object> { { "@Email", email } };
            var result = _db.ExecuteSelect(query, parameters);
            return Convert.ToInt32(result.Rows[0][0]) > 0;
        }

        public bool ResetPassword(string email, string token, string newPassword)
        {
            string query = @"UPDATE usuarios 
                           SET senha_hash = @Password, 
                               token_recuperacao = NULL, 
                               expiracao_token = NULL 
                           WHERE email = @Email 
                           AND token_recuperacao = @Token 
                           AND expiracao_token > NOW()";

            var parameters = new Dictionary<string, object>
            {
                { "@Email", email },
                { "@Token", token },
                { "@Password", HashPassword(newPassword) }
            };

            return _db.ExecuteUpdate(query, parameters) > 0;
        }
    }
}