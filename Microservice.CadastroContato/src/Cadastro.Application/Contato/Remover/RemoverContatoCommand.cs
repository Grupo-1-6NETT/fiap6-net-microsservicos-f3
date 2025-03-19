using MediatR;

namespace Cadastro.Application.Contato.Remover;

public class RemoverContatoCommand : IRequest<Guid>
{
    public Guid Id { get; }
    public RemoverContatoCommand(Guid id)
    {
        Id = id;
    }



}
