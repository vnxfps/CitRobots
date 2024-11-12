using CitRobots.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

[Authorize]
public class UsuarioController : Controller
{
    private readonly CitRobotsContext _context;
    private readonly IPasswordHasher<Usuario> _passwordHasher;

    public UsuarioController(CitRobotsContext context, IPasswordHasher<Usuario> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == model.Email);

            if (usuario != null && _passwordHasher.VerifyHashedPassword(
                usuario, usuario.Senha, model.Senha) == PasswordVerificationResult.Success)
            {
                await SignInUser(usuario);
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Login inválido");
        }
        return View(model);
    }

    [AllowAnonymous]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var usuario = new Usuario
            {
                Email = model.Email,
                Senha = _passwordHasher.HashPassword(null, model.Senha)
            };

            var cliente = new Cliente
            {
                Nome = model.Nome,
                Sobrenome = model.Sobrenome,
                Email = model.Email,
                CPF = model.CPF,
                DataNascimento = model.DataNascimento,
                Endereco = model.Endereco,
                Usuario = usuario
            };

            _context.Usuarios.Add(usuario);
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            await SignInUser(usuario);
            return RedirectToAction("Index", "Home");
        }
        return View(model);
    }

    private async Task SignInUser(Usuario usuario)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, usuario.Email),
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString())
        };

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
    }
}