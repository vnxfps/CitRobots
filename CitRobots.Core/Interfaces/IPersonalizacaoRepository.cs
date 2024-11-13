public interface IPersonalizacaoRepository : IBaseRepository<Personalizacao>
{
    Task<IEnumerable<Personalizacao>> GetByTipoAsync(string tipo);
    Task<bool> IsCompatibleWithRobotAsync(int personalizacaoId, int roboId);
}