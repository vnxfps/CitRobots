public interface IUsuarioRepository : IBaseRepository<Usuario>
{
    Task<Usuario> GetByEmailAsync(string email);
    Task<bool> EmailExistsAsync(string email);
    Task<bool> ValidateCredentialsAsync(string email, string senha);
}