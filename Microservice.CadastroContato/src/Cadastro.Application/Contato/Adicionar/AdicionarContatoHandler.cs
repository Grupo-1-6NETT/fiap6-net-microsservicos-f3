using Cadastro.Application.Services;
using MediatR;

namespace Cadastro.Application.Contato.Adicionar;
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

        var contato = new Domain.Models.Contato
        {
            Id = Guid.NewGuid(),
            Nome = request.Nome,
            Telefone = request.Telefone,
            DDD = request.DDD,
            Email = request.Email,
        };

        _rabbitmq.PublicarMensagem(contato, "contatos.adicionar");

        return Task.FromResult(contato.Id);
    }
}
