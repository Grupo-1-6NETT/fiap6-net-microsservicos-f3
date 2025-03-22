using Application.Services;
using Domain;
using MediatR;

namespace Application.Contato;

public class AdicionarContatoHandler : IRequestHandler<AdicionarContatoCommand, Guid>
{
    private readonly IRabbitMQService _rabbitmq;

    public AdicionarContatoHandler(IRabbitMQService rabbitmq)
    {
        _rabbitmq = rabbitmq;
    }

    public Task<Guid> Handle(AdicionarContatoCommand request, CancellationToken cancellationToken)
    {
    
        request.Validate();

        var contato = new AdicionarContatoDto()
        { 
            TransportId= Guid.NewGuid(),
            Nome = request.Nome,
            Telefone = request.Telefone,
            DDD = request.DDD,
            Email = request.Email,
        };

        _rabbitmq.PublicarMensagem(contato);

        return Task.FromResult(contato.TransportId);
    }
}
