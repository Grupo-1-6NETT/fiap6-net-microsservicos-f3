using MassTransit;
using Microsoft.Extensions.Configuration;

namespace Application.Services;

public class RabbitMQService : IRabbitMQService
{
    private readonly string _queue;
    private readonly IBus _bus;
    
    public RabbitMQService(IConfiguration config, IBus bus)
    {
        _bus = bus;
        _queue = config["RabbitMQ:QueueName"] ?? string.Empty;
    }
    public async Task PublicarMensagem<T>(T mensagem)
    {
        if(mensagem == null)
            return;
        
        var endpoint = await _bus.GetSendEndpoint(new Uri($"queue:{_queue}"));        
        await endpoint.Send(mensagem);
    }
}
