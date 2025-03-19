using Cadastro.Application.Services;
using MediatR;

namespace Cadastro.Application.Contato.Consultar;
public class ConsultarContatoHandler : IRequestHandler<ConsultarContatoQuery, IEnumerable<Domain.Models.Contato>>
{
    private readonly IRabbitMQService _rabbitmq;

    public ConsultarContatoHandler(IRabbitMQService rabbitmq)
    {
        _rabbitmq = rabbitmq;
    }

    public Task<IEnumerable<Domain.Models.Contato>> Handle(ConsultarContatoQuery request, CancellationToken cancellationToken)
    {
        
        _rabbitmq.PublicarMensagem(request, "contatos.consulta");
        return null;
    }
}
