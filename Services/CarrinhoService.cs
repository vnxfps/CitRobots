using Microsoft.EntityFrameworkCore;

public class CarrinhoService : CarrinhoServiceBase, ICarrinhoService
{
    private readonly CitRobotsContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CarrinhoService(
        CitRobotsContext context,
        IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<CarrinhoViewModel> GetCarrinhoAsync(string userId)
    {
        var carrinho = await _context.Carrinhos
            .Include(c => c.Items).ThenInclude(i => i.Robot).ThenInclude(i => i.Personalizacoes).FirstOrDefaultAsync(c => c.UserId == userId);

        return MapToViewModel(carrinho);
    }

    private CarrinhoViewModel MapToViewModel(object carrinho)
    {
        throw new NotImplementedException();
    }

    public async void GetCarrinho(string userId) => await GetOrCreateCarrinhoAsync(userId);

    private async Task GetOrCreateCarrinhoAsync(string userId)
    {
        throw new NotImplementedException();
    }

    Task<CarrinhoViewModel> ICarrinhoService.GetCarrinhoAsync(string userId)
    {
        throw new NotImplementedException();
    }

    Task ICarrinhoService.AddItemAsync(string userId, int robotId, List<int> personalizacaoIds)
    {
        throw new NotImplementedException();
    }

    Task ICarrinhoService.RemoveItemAsync(string userId, int itemId)
    {
        throw new NotImplementedException();
    }

    Task ICarrinhoService.ClearCarrinhoAsync(string userId)
    {
        throw new NotImplementedException();
    }

    Task ICarrinhoService.ProcessarPedidoAsync(string userId, EnderecoEntrega endereco, string formaPagamento)
    {
        throw new NotImplementedException();
    }

    Task ICarrinhoService.ProcessarPedidoAsync(string? name, EnderecoEntrega enderecoEntrega, object formaPagamento)
    {
        throw new NotImplementedException();
    }
}