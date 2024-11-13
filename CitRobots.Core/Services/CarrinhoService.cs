using System.Text.Json;

public class CarrinhoService : ICarrinhoService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRoboRepository _roboRepository;
    private readonly IPersonalizacaoRepository _personalizacaoRepository;

    private const string CarrinhoSessionKey = "CarrinhoCompras";

    public CarrinhoService(
        IHttpContextAccessor httpContextAccessor,
        IRoboRepository roboRepository,
        IPersonalizacaoRepository personalizacaoRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _roboRepository = roboRepository;
        _personalizacaoRepository = personalizacaoRepository;
    }

    public async Task<CarrinhoViewModel> AdicionarItemAsync(AdicionarItemCarrinhoCommand command)
    {
        var carrinho = ObterCarrinho();
        var robo = await _roboRepository.GetByIdAsync(command.RoboId);

        if (robo == null || robo.Quantidade <= 0)
            throw new InvalidOperationException("Robô não disponível");

        var item = new ItemCarrinhoViewModel
        {
            RoboId = robo.Id,
            Nome = robo.Nome,
            Preco = robo.Preco,
            Personalizacoes = await ObterPersonalizacoesAsync(command.PersonalizacaoIds)
        };

        carrinho.Items.Add(item);
        carrinho.Total = carrinho.Items.Sum(i => i.Preco + i.Personalizacoes.Sum(p => p.Preco));

        SalvarCarrinho(carrinho);
        return carrinho;
    }

    private CarrinhoViewModel ObterCarrinho()
    {
        var session = _httpContextAccessor.HttpContext.Session;
        var carrinhoJson = session.GetString(CarrinhoSessionKey);

        return carrinhoJson == null
            ? new CarrinhoViewModel()
            : JsonSerializer.Deserialize<CarrinhoViewModel>(carrinhoJson);
    }

    private void SalvarCarrinho(CarrinhoViewModel carrinho)
    {
        var session = _httpContextAccessor.HttpContext.Session;
        session.SetString(CarrinhoSessionKey, JsonSerializer.Serialize(carrinho));
    }
}