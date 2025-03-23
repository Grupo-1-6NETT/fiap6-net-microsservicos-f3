using Domain;
using Microsoft.Extensions.Configuration;
using MassTransit;

namespace Application.Services;

public class RabbitMQService(IConfiguration config, IBus bus) : IRabbitMQService
{
    private readonly string _queue = config["RabbitMQ:QueueName"] ?? string.Empty;

    public async Task PublicarMensagem(RemoverContatoDto mensagem)
    {        
        var endpoint = await bus.GetSendEndpoint(new Uri($"queue:{_queue}"));        
        await endpoint.Send(mensagem);
    }
}
