using Application.Services;
using Domain;
using MediatR;

namespace Application.Contato;

public class RemoverContatoHandler(IRabbitMQService rabbitmq) : IRequestHandler<RemoverContatoCommand, Guid>
{
    public async Task<Guid> Handle(RemoverContatoCommand request, CancellationToken cancellationToken)
    {
        var contato = new RemoverContatoDto { ContatoId = request.Id };
        
        await rabbitmq.PublicarMensagem(contato);

        return request.Id;
    }
}
