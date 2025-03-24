using Application.Contato;
using Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
[Route("[controller]")]
[ApiController]
public class AtualizarContatoController(IMediator mediator) : ControllerBase
{
    [Authorize]
    [HttpPatch]
    /// <summary>
    /// Atualiza um Contato na base de dados 
    /// </summary>
    /// <remarks>
    /// Exemplo:
    /// 
    ///  {
    ///     "id": "1991dcff-06a9-4b09-9e16-79f76055a21b",
    ///     "nome": "João",
    ///     "telefone": "988994199",
    ///     "ddd": "11",
    ///     "email": "joao@gmail.com"
    /// }
    /// </remarks>
    /// <param name="command">Comando com os dados do Contato</param>
    /// <returns>O Id do Contato atualizado</returns>
    /// <response code="200">Contato atualizado na base de dados</response>
    /// <response code="400">Falha ao processar a requisição</response>
    /// <response code="401">Usuário não autenticado</response>
    /// <response code="403">Usuário não autorizado</response>
    /// <response code="404">Contato não encontrado</response>
    /// <response code="500">Erro inesperado</response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult Patch([FromBody] AtualizarContatoCommand request)
    {
        try
        {
            var contatoId = mediator.Send(request);
            return Ok(contatoId);
        }
        catch (ContatoValidationException ex)
        {
            return BadRequest(new { Erros = ex.Errors });
        }
        catch (Exception)
        {
            return StatusCode(500, new { Message = "Ocorreu um erro interno no servidor." });
        }          
    }
}