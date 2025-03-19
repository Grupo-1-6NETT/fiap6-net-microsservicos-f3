using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Cadastro.Application.Services;

public class RabbitMQService : IRabbitMQService, IDisposable
{
    private readonly IConnectionFactory _factory;
    private IChannel _channel;
    private IConnection _connection;
    public RabbitMQService(IConfiguration config)
    {
        _factory = new ConnectionFactory()
        {
            HostName = config["RabbitMQ:HostName"],
            UserName = config["RabbitMQ:UserName"],
            Password = config["RabbitMQ:Password"]
        };
        Setup().Wait();
    }
    public async Task PublicarMensagem<T>(T mensagem, string queueName)
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
                { "x-dead-letter-routing-key", "contatos.dlq" }
            }
        };
        await _channel.BasicPublishAsync(            
            exchange: "contatos.exchange",
            routingKey: queueName,
            mandatory: true,
            basicProperties: props,
            body: body);
    }
    private async Task Setup()
    {
        _connection = await _factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();

        await _channel.ExchangeDeclareAsync(
            exchange: "contatos.exchange",
            type: ExchangeType.Direct,
            durable: true,
            autoDelete: false
        );

        await _channel.QueueDeclareAsync(
            queue: "contatos.consulta",
            durable: true,
            exclusive: false,
            autoDelete: false
        );
        await _channel.QueueBindAsync(
        queue: "contatos.consulta",
        exchange: "contatos.exchange",
        routingKey: "contatos.consulta"
    );
        await _channel.QueueDeclareAsync(
            queue: "contatos.adicionar",
            durable: true,
            exclusive: false,
            autoDelete: false
        );
        await _channel.QueueBindAsync(
        queue: "contatos.adicionar",
        exchange: "contatos.exchange",
        routingKey: "contatos.adicionar"
    );
        await _channel.QueueDeclareAsync(
            queue: "contatos.remover",
            durable: true,
            exclusive: false,
            autoDelete: false
        );
        await _channel.QueueBindAsync(
                queue: "contatos.remover",
                exchange: "contatos.exchange",
                routingKey: "contatos.remover"
            );

        await _channel.QueueDeclareAsync(
            queue: "contatos.atualizar",
            durable: true,
            exclusive: false,
            autoDelete: false
        );

        await _channel.QueueBindAsync(
        queue: "contatos.atualizar",
        exchange: "contatos.exchange",
        routingKey: "contatos.atualizar"
    );

        await _channel.QueueDeclareAsync(
            queue: "contatos.dlq",
            durable: true,
            exclusive: false,
            autoDelete: false
        );

    }

    public async void Dispose()
    {
        await _channel.CloseAsync();
        await _connection.CloseAsync();
    }

}
