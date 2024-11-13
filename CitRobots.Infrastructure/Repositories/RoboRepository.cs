using Microsoft.EntityFrameworkCore;

public class RoboRepository : BaseRepository<Robo>, IRoboRepository
{
    public RoboRepository(CitRobotsContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Robo>> GetAvailableRobotsAsync()
    {
        return await _dbSet
            .Where(r => !r.Deletado && r.Quantidade > 0)
            .ToListAsync();
    }

    public async Task<bool> UpdateEstoqueAsync(int roboId, int quantidade)
    {
        var robo = await GetByIdAsync(roboId);
        if (robo == null) return false;

        robo.Quantidade = quantidade;
        robo.AtualizadoEm = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return true;
    }
}