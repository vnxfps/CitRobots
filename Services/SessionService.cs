using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitRobots.Models;

namespace CitRobots.Services
{
    public static class SessionService
    {
        private static SessionModel _currentSession;

        public static SessionModel CurrentSession
        {
            get { return _currentSession ?? new SessionModel(); }
            private set { _currentSession = value; }
        }

        public static void CreateSession(int userId, string nome, string email, string tipoUsuario)
        {
            CurrentSession = new SessionModel
            {
                UserId = userId,
                Nome = nome,
                Email = email,
                TipoUsuario = tipoUsuario
            };
        }

        public static void ClearSession()
        {
            CurrentSession = null;
        }

        public static bool IsLoggedIn()
        {
            return CurrentSession?.IsAuthenticated ?? false;
        }
    }
}