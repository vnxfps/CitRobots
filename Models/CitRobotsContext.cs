using CitRobots.Models;
using Microsoft.EntityFrameworkCore;

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
    public object Carrinhos { get; internal set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configurações de relacionamentos e restrições aqui
        modelBuilder.Entity<RobotPersonalizacao>()
            .HasKey(rp => new { rp.RoboId, rp.PersonalizacaoId });

        modelBuilder.Entity<Robot>()
            .Property(r => r.Preco)
            .HasColumnType("decimal(10,2)");
    }
}