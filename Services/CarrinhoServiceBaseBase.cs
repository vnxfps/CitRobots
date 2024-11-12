public class CarrinhoServiceBase : ICarrinhoServiceBase
{

    public async Task AddItemAsync(string userId, int robotId, List<int> personalizacaoIds, Carrinho carrinho)
    {
        if (string.IsNullOrEmpty(userId))
        {
            throw new ArgumentException($"'{nameof(userId)}' cannot be null or empty.", nameof(userId));
        }

        if (personalizacaoIds is null)
        {
            throw new ArgumentNullException(nameof(personalizacaoIds));
        }

        var robot = await _context.Robots.FindAsync(robotId);
        if (robot == null)
        {
            throw new ArgumentException($"Robot with ID {robotId} not found.", nameof(robotId));
        }

        var item = new CarrinhoItem
        {
            RobotId = robotId,
            Preco = robot.Preco
        };

        foreach (var personalizacaoId in personalizacaoIds)
        {
            var personalizacao = await _context.Personalizacoes.FindAsync(personalizacaoId);
            if (personalizacao != null)
            {
                item.Personalizacoes.Add(personalizacao);
                item.Preco += personalizacao.Preco;
            }
        }

        carrinho.Items.Add(item);
        await _context.SaveChangesAsync();
    }
}