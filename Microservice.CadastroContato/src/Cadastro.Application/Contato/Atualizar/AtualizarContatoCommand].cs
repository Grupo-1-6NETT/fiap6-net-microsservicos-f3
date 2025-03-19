using Cadastro.Application.Contato.Adicionar;

namespace Cadastro.Application.Contato.Atualizar;
public class AtualizarContatoCommand : AdicionarContatoCommand
{
    public Guid id { get; set; }
}
