using Cadastro.Application.Contato.Adicionar;
using Cadastro.Application.Contato.Atualizar;
using Cadastro.Application.Contato.Consultar;
using Cadastro.Application.Contato.Remover;
using Cadastro.Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Cadastro.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ContatoController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ContatoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        /// <summary>
        /// Lista os Contatos cadastrados, ordenados por nome, que correspondem aos parâmetros informados
        /// </summary>    
        /// <param name="ddd">Filtra os contatos cujo telefone contenham o DDD informado</param>
        /// <param name="pagina">Informe o número de página para ignorar os contatos de páginas anteriores. Se não informado, todos os contatos serão exibidos</param>
        /// <param name="resultadosPorPagina">Número de contatos a serem exibidos para a <paramref name="pagina"/> informada. Se não informado, todos os contatos serão exibidos</param>
        /// <returns>A lista de Contatos correspodentes à pesquisa</returns>
        /// <response code="200">Pesquisa realizada com sucesso</response>
        /// <response code="401">Usuário não autenticado</response>    
        /// <response code="500">Erro inesperado</response>
        public IActionResult Listar(string? ddd = null, int? pagina = null, int? resultadosPorPagina = null)
        {
            var query = new ConsultarContatoQuery { DDD = ddd, PageIndex = pagina, PageSize = resultadosPorPagina };
            var contatos = _mediator.Send(query);
            return Ok(contatos);
        }

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
        public IActionResult Adicionar([FromBody] AdicionarContatoCommand request)
        {
            try
            {
            var contatoId = _mediator.Send(request);
            return Created("", contatoId);

            }
            catch(ContatoValidationException ex)
            {
                return BadRequest(new { Erros = ex.Errors });
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "Ocorreu um erro interno no servidor." });
            }

        }

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
        public IActionResult Atualizar([FromBody] AtualizarContatoCommand request)
        {
            try
            {
                var contatoId = _mediator.Send(request);
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
        public async Task<IActionResult> Remover(Guid id)
        {
            var command = new RemoverContatoCommand(id);
            await _mediator.Send(command);

            return Ok($"Contato com {id} removido com sucesso.");
        }
    }
}
