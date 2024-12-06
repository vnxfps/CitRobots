using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitRobots.Models
{
    public class SessionModel
    {
        public int UserId { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string TipoUsuario { get; set; }
        public bool IsAuthenticated => UserId > 0;
    }
}