using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class AuthenticationService : IAuthenticationService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthenticationService> _logger;

    public AuthenticationService(
        IUsuarioRepository usuarioRepository,
        IConfiguration configuration,
        ILogger<AuthenticationService> logger)
    {
        _usuarioRepository = usuarioRepository;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<string> AuthenticateAsync(string email, string senha)
    {
        try
        {
            var usuario = await _usuarioRepository.GetByEmailAsync(email);
            if (usuario == null || !VerifyPassword(senha, usuario.Senha, usuario.Salt))
                throw new UnauthorizedException("Credenciais inválidas");

            return GenerateJwtToken(usuario);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during authentication");
            throw;
        }
    }

    private string GenerateJwtToken(Usuario usuario)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Login)
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private bool VerifyPassword(string senha, string hashedSenha, string salt)
    {
        var hashToCompare = HashPassword(senha, salt);
        return hashedSenha == hashToCompare;
    }

    private string HashPassword(string senha, string salt)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(senha + salt);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}