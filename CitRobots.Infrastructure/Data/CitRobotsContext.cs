using Microsoft.EntityFrameworkCore;

public class CitRobotsContext : DbContext
{
    public CitRobotsContext(DbContextOptions<CitRobotsContext> options) : base(options)
    {
    }

    public DbSet<Robo> Robos { get; set; }
    public DbSet<Personalizacao> Personalizacoes { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<ItemPedido> ItensPedido { get; set; }
    public DbSet<CupomDesconto> CuponsDesconto { get; set; }
    public DbSet<Pagamento> Pagamentos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurações de índices
        modelBuilder.Entity<Robo>()
            .HasIndex(r => r.Nome);

        modelBuilder.Entity<Personalizacao>()
            .HasIndex(p => p.Nome);

        modelBuilder.Entity<Cliente>()
            .HasIndex(c => c.Email)
            .IsUnique();

        modelBuilder.Entity<Cliente>()
            .HasIndex(c => c.CPF)
            .IsUnique();

        // Configurações de relacionamentos
        modelBuilder.Entity<Cliente>()
            .HasOne(c => c.Usuario)
            .WithOne(u => u.Cliente)
            .HasForeignKey<Cliente>(c => c.UsuarioId);

        modelBuilder.Entity<Pedido>()
            .HasOne(p => p.Cliente)
            .WithMany(c => c.Pedidos)
            .HasForeignKey(p => p.ClienteId);

        modelBuilder.Entity<ItemPedido>()
            .HasOne(i => i.Pedido)
            .WithMany(p => p.ItensPedido)
            .HasForeignKey(i => i.PedidoId);

        // Configurações de validações
        modelBuilder.Entity<Robo>()
            .Property(r => r.Preco)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Personalizacao>()
            .Property(p => p.Preco)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Pedido>()
            .Property(p => p.Total)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Pagamento>()
            .Property(p => p.ValorTotal)
            .HasPrecision(10, 2);
    }
}