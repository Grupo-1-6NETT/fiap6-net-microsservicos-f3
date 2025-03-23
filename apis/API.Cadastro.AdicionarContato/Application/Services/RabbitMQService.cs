using Domain;
using MassTransit;
using Microsoft.Extensions.Configuration;

namespace Application.Services;

public class RabbitMQService(IConfiguration config, IBus bus) : IRabbitMQService
{
    private readonly string _queue = config["RabbitMQ:QueueName"] ?? string.Empty;

    public async Task PublicarMensagem(AdicionarContatoDto mensagem)
    {
        
        var endpoint = await bus.GetSendEndpoint(new Uri($"queue:{_queue}"));        
        await endpoint.Send(mensagem);
    }
}
