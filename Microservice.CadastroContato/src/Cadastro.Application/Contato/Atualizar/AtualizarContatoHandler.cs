using Cadastro.Application.Services;
using MediatR;

namespace Cadastro.Application.Contato.Atualizar;
public class AtualizarContatoHandler : IRequestHandler<AtualizarContatoCommand, Guid>
{
    private readonly IRabbitMQService _rabbitmq;

    public AtualizarContatoHandler(IRabbitMQService rabbitmq)
    {
        _rabbitmq = rabbitmq;
    }

    public Task<Guid> Handle(AtualizarContatoCommand request, CancellationToken cancellationToken)
    {
        request.Validate();

        _rabbitmq.PublicarMensagem(request, "contatos.atualizar");

        return Task.FromResult(request.id);
    }
}
