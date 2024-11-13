using Microsoft.EntityFrameworkCore;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Adiciona serviços ao container
        builder.Services.AddControllersWithViews();
        builder.Services.AddDbContext<CitRobotsContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Registra serviços
        builder.Services.AddScoped<IRoboRepository, RoboRepository>();
        builder.Services.AddScoped<IPersonalizacaoRepository, PersonalizacaoRepository>();
        builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
        builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
        builder.Services.AddScoped<ICupomDescontoRepository, CupomDescontoRepository>();
        builder.Services.AddScoped<IPagamentoRepository, PagamentoRepository>();

        builder.Services.AddScoped<IRoboService, RoboService>();
        builder.Services.AddScoped<ICarrinhoService, CarrinhoService>();
        builder.Services.AddScoped<IPersonalizacaoService, PersonalizacaoService>();
        builder.Services.AddScoped<IUsuarioService, UsuarioService>();
        builder.Services.AddSc