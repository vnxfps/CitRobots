public interface IRoboRepository : IBaseRepository<Robo>
{
    Task<IEnumerable<Robo>> GetAvailableRobotsAsync();
    Task<bool> UpdateEstoqueAsync(int roboId, int quantidade);
}