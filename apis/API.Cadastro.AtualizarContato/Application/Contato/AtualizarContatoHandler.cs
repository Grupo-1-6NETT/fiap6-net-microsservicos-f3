using Application.Services;
using Domain;
using MediatR;

namespace Application.Contato;

public class AtualizarContatoHandler(IRabbitMQService rabbitmq) : IRequestHandler<AtualizarContatoCommand, Guid>
{
    public Task<Guid> Handle(AtualizarContatoCommand request, CancellationToken cancellationToken)
    {
        request.Validate();
        
        var contato = new AtualizarContatoDto()
        { 
            ContatoId = request.Id,
            Nome = request.Nome,
            Telefone = request.Telefone,
            DDD = request.DDD,
            Email = request.Email,
        };
        
        rabbitmq.PublicarMensagem(contato);

        return Task.FromResult(contato.ContatoId);
    }
}
