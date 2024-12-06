using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitRobots.Helpers;
using CitRobots.Models;

namespace CitRobots.Services
{
    public static class SessionService
    {
        private static SessionModel _currentSession;
        private static readonly DatabaseHelper _db = new DatabaseHelper();

        public static SessionModel CurrentSession => _currentSession ?? new SessionModel();

        public static void CreateSession(int userId, string nome, string email, string tipoUsuario)
        {
            _currentSession = new SessionModel
            {
                UserId = userId,
                Nome = nome,
                Email = email,
                TipoUsuario = tipoUsuario
            };
            LogSession(userId, "LOGIN");
        }

        public static void ClearSession()
        {
            if (_currentSession != null)
            {
                LogSession(_currentSession.UserId, "LOGOUT");
                _currentSession = null;
            }
        }

        private static void LogSession(int userId, string tipo)
        {
            string query = "INSERT INTO logs_sistema (tipo, usuario_id, data_criacao) VALUES (@Tipo, @UserId, NOW())";
            var parameters = new Dictionary<string, object>
        {
            { "@Tipo", tipo },
            { "@UserId", userId }
        };
            _db.ExecuteInsert(query, parameters);
        }

        public static bool IsAdmin() =>
            CurrentSession?.TipoUsuario?.Equals("ADMIN", StringComparison.OrdinalIgnoreCase) ?? false;

        public static bool IsAuthenticated() => CurrentSession?.IsAuthenticated ?? false;
    }
}