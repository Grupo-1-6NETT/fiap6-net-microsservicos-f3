using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
public class GatewayController : ControllerBase
{
    /// <summary>
    /// Obtém dados protegidos da API.TESTE
    /// </summary>
    [HttpGet("teste")]
    [Authorize] // Protegido pelo token
    public IActionResult GetTeste()
    {
        return Ok("Este é um endpoint protegido da API.TESTE");
    }

    /// <summary>
    /// Obtém um token de autenticação (API.AUTH.TOKEN)
    /// </summary>
    [HttpGet("token")]
    public IActionResult GetToken()
    {
        return Ok("Endpoint para obter um token de autenticação");
    }

    /// <summary>
    /// Cria um novo usuário na API.AUTH.USUARIO
    /// </summary>
    [HttpPost("usuario")]
    public IActionResult PostUsuario()
    {
        return Ok("Endpoint para criar um usuário");
    }

    /// <summary>
    /// Deleta um usuário (proteção por token)
    /// </summary>
    [HttpDelete("usuario")]
    [Authorize] // Protegido pelo token
    public IActionResult DeleteUsuario()
    {
        return Ok("Endpoint para deletar um usuário protegido por autenticação");
    }
}
