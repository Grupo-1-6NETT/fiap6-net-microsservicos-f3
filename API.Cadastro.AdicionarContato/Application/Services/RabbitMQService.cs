using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Application.Services;

public class RabbitMQService : IRabbitMQService
{
    private readonly string _queue;
    private readonly string _exchange;    
    private readonly string _dlq;

    private readonly IConnectionFactory _factory;
    private IChannel _channel;
    private IConnection _connection;
    
    public RabbitMQService(IConfiguration config)
    {
        _factory = new ConnectionFactory()
        {
            HostName = config["RabbitMQ:HostName"] ?? string.Empty,
            UserName = config["RabbitMQ:UserName"] ?? string.Empty,
            Password = config["RabbitMQ:Password"] ?? string.Empty,
        };
        _queue = config["RabbitMQ:QueueName"] ?? string.Empty;
        _exchange = config["RabbitMQ:Exchange"] ?? string.Empty;
        _dlq = config["RabbitMQ:DeadLetterQueue"] ?? string.Empty;
        
        Setup().Wait();
    }
    public async Task PublicarMensagem<T>(T mensagem)
    {
        var json = JsonSerializer.Serialize(mensagem);
        var body = Encoding.UTF8.GetBytes(json);

        var props = new BasicProperties
        {
            DeliveryMode = DeliveryModes.Persistent,
            ContentType = "application/json",
            Headers = new Dictionary<string, object>
            {
                { "x-dead-letter-exchange", "" },
                { "x-dead-letter-routing-key", _dlq}
            }!
        };
        
        await _channel.BasicPublishAsync(            
            exchange: _exchange,
            routingKey: _queue,
            mandatory: true,
            basicProperties: props,
            body: body);
    }
    
    private async Task Setup()
    {
        _connection = await _factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();

        await _channel.ExchangeDeclareAsync(
            exchange: _exchange,
            type: ExchangeType.Direct,
            durable: true,
            autoDelete: false
        );

        await _channel.QueueDeclareAsync(
            queue: _queue,
            durable: true,
            exclusive: false,
            autoDelete: false
        );
        await _channel.QueueBindAsync(
        queue: _queue,
        exchange: _exchange,
        routingKey: _queue
    );

        await _channel.QueueDeclareAsync(
            queue: _dlq,
            durable: true,
            exclusive: false,
            autoDelete: false
        );

    }
}
