using Microsoft.EntityFrameworkCore;
using CitRobots.Models;

namespace CitRobots.Data
{
    public class CitRobotsContext : DbContext
    {
        public CitRobotsContext(DbContextOptions<CitRobotsContext> options)
            : base(options)
        {
        }

        public DbSet<Robot> Robots { get; set; }
        public DbSet<Personalizacao> Personalizacoes { get; set; }
        public DbSet<RobotPersonalizacao> RobotsPersonalizacoes { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<ItemPedido> ItensPedido { get; set; }
        public DbSet<CupomDesconto> CuponsDesconto { get; set; }
        public DbSet<Pagamento> Pagamentos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurações aqui
        }
    }
}