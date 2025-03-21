using Cadastro.Gateway.API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IConfiguration _config;

    /// <summary>
    /// Construtor do controlador de autenticação.
    /// </summary>
    /// <param name="userManager">Gerenciador de usuários.</param>
    /// <param name="signInManager">Gerenciador de login.</param>
    /// <param name="config">Configurações da aplicação.</param>
    public AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration config)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _config = config;
    }

    /// <summary>
    /// Realiza o cadastro de um novo usuário.
    /// </summary>
    /// <param name="usuario">Dados do usuário a ser cadastrado.</param>
    /// <returns>Retorna sucesso ou erro no cadastro.</returns>
    /// <response code="200">Usuário registrado com sucesso.</response>
    /// <response code="400">Erro ao registrar o usuário.</response>
    [HttpPost("cadastro")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] Usuario usuario)
    {
        if (usuario == null) return BadRequest("Dados do usuário inválidos.");

        var user = new IdentityUser { UserName = usuario.Nome };
        var result = await _userManager.CreateAsync(user, usuario.Senha);

        if (result.Succeeded)
            return Ok(new { mensagem = "Usuário registrado com sucesso." });

        return BadRequest(new { erros = result.Errors });
    }

    /// <summary>
    /// Realiza o login do usuário e gera um token JWT.
    /// </summary>
    /// <param name="usuario">Credenciais do usuário.</param>
    /// <returns>Retorna o token JWT se a autenticação for bem-sucedida.</returns>
    /// <response code="200">Login realizado com sucesso.</response>
    /// <response code="401">Nome ou senha incorreta.</response>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] Usuario usuario)
    {
        if (usuario == null) return BadRequest("Dados do usuário inválidos.");

        var user = await _userManager.FindByNameAsync(usuario.Nome);
        if (user == null) return Unauthorized(new { mensagem = "Nome ou senha incorreta." });

        var result = await _signInManager.CheckPasswordSignInAsync(user, usuario.Senha, false);
        if (!result.Succeeded) return Unauthorized(new { mensagem = "Nome ou senha incorreta." });

        var token = GenerateJwtToken(user);
        return Ok(new { token });
    }

    /// <summary>
    /// Gera um token JWT para o usuário autenticado.
    /// </summary>
    /// <param name="user">Usuário autenticado.</param>
    /// <returns>Token JWT válido.</returns>
    private string GenerateJwtToken(IdentityUser user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
