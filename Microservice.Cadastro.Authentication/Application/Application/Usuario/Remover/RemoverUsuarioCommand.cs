using MediatR;

namespace Cadastro.Auth.Application;

public class RemoverUsuarioCommand : IRequest<Guid>
{
    public Guid UsuarioId { get; }
    public RemoverUsuarioCommand(Guid id)
    {
        UsuarioId = id;
    }        
}
