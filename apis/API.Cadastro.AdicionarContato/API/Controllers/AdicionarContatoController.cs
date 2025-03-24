using Application.Contato;
using Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
[Route("[controller]")]
[ApiController]
public class AdicionarContatoController(IMediator mediator) : ControllerBase
{
    [Authorize]
    [HttpPost]
    /// <summary>
    /// Adiciona um Contato na base de dados 
    /// </summary>
    /// <remarks>
    /// Exemplo:
    /// 
    ///  {
    ///     "nome": "João",
    ///     "telefone": "988994199",
    ///     "ddd": "11",
    ///     "email": "joao@gmail.com"
    /// }
    /// </remarks>
    /// <param name="command">Comando com os dados do Contato</param>
    /// <returns>O Id do Contato adicionado</returns>
    /// <response code="201">Contato adicionado na base de dados</response>
    /// <response code="400">Falha ao processar a requisição</response>
    /// <response code="401">Usuário não autenticado</response>
    /// <response code="403">Usuário não autorizado</response>
    /// <response code="500">Erro inesperado</response>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult Post([FromBody] AdicionarContatoCommand request)
    {
        try
        {
            var contatoId = mediator.Send(request);
            return Created("", contatoId);
        }
        catch (ContatoValidationException ex)
        {
            return BadRequest(new { Erros = ex.Errors });
        }
        catch (Exception)
        {
            return StatusCode(500, new { Message = $"Ocorreu um erro interno no servidor." });
        }
    }
}