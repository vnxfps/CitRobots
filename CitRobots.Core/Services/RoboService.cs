public class RoboService : IRoboService
{
    private readonly IRoboRepository _roboRepository;

    public RoboService(IRoboRepository roboRepository)
    {
        _roboRepository = roboRepository;
    }

    public async Task<IEnumerable<RoboViewModel>> GetAllRobosAsync()
    {
        var robos = await _roboRepository.GetAvailableRobotsAsync();
        return robos.Select(r => new RoboViewModel
        {
            Id = r.Id,
            Nome = r.Nome,
            Preco = r.Preco,
            Slogan = r.Slogan,
            Descricao = r.Descricao,
            ImagemUrl = r.ImagemUrl,
            Disponivel = r.Quantidade > 0
        });
    }

    public async Task<RoboDetalhesViewModel> GetRoboDetalhesAsync(int id)
    {
        var robo = await _roboRepository.GetByIdAsync(id);
        if (robo == null) return null;

        return new RoboDetalhesViewModel
        {
            Id = robo.Id,
            Nome = robo.Nome,
            Preco = robo.Preco,
            Slogan = robo.Slogan,
            Descricao = robo.Descricao,
            ImagemUrl = robo.ImagemUrl,
            Disponivel = robo.Quantidade > 0,
            Especificacoes = new List<string>
            {
                "Inteligência Artificial Avançada",
                "Sistema de Visão 360°",
                "Sensores de Alta Precisão",
                "Bateria de Longa Duração"
            }
        };
    }
}