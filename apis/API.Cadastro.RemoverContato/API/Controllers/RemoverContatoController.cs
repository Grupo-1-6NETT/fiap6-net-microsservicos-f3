using Application.Contato;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("[controller]")]
[ApiController]
public class RemoverContatoController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    
    [HttpDelete("{id}")]
    /// <summary>
    /// Remove o contato na base de dados com o ID informado
    /// </summary>
    /// <param name="id">O ID do contato a ser removido</param>
    /// <returns>Resultado da operação de remoção</returns>
    /// <response code="200">Contato removido com sucesso</response>
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
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new RemoverContatoCommand(id);
        await _mediator.Send(command);

        return Ok($"Contato com {id} enviado para remoção.");
    }
}