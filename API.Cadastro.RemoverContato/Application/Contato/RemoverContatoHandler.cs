using Application.Services;
using MediatR;

namespace Application.Contato;

public class RemoverContatoHandler(IRabbitMQService rabbitmq) : IRequestHandler<RemoverContatoCommand, Guid>
{
    public async Task<Guid> Handle(RemoverContatoCommand request, CancellationToken cancellationToken)
    {
        await rabbitmq.PublicarMensagem(request);

        return request.Id;
    }
}
