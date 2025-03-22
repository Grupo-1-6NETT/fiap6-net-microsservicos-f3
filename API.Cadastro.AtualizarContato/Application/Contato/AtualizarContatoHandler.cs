using Application.Services;
using MediatR;

namespace Application.Contato;

public class AtualizarContatoHandler(IRabbitMQService rabbitmq) : IRequestHandler<AtualizarContatoCommand, Guid>
{
    public Task<Guid> Handle(AtualizarContatoCommand request, CancellationToken cancellationToken)
    {
        request.Validate();
        //TODO: validar existência do contato aqui?
        rabbitmq.PublicarMensagem(request);

        return Task.FromResult(request.Id);
    }
}
