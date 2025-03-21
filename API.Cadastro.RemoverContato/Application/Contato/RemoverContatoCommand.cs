using MediatR;

namespace Application.Contato;

public class RemoverContatoCommand(Guid id) : IRequest<Guid>
{
    public Guid Id { get; } = id;
}
