using Cadastro.Application.Services;
using MediatR;

namespace Cadastro.Application.Contato.Remover;
public class RemoverContatoHandler : IRequestHandler<RemoverContatoCommand, Guid>
{
    private readonly IRabbitMQService _rabbitmq;

    public RemoverContatoHandler(IRabbitMQService rabbitmq)
    {
        _rabbitmq = rabbitmq;
    }

    public async Task<Guid> Handle(RemoverContatoCommand request, CancellationToken cancellationToken)
    {
        await _rabbitmq.PublicarMensagem(request,"contatos.remover");

        return request.Id;
    }
}
