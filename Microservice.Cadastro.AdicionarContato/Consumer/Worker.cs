using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using ContatoDb.Core.Interfaces;
using ContatoDb.Core.Models;

namespace Consumer;
public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConnectionFactory _factory;
    private readonly string _queueName;
    private IChannel _channel;
    private IConnection _connection;
    private IContatoRepository _contatoRepository;

    public Worker(IConfiguration config, ILogger<Worker> logger, IContatoRepository contatoRepository)
    {
        _contatoRepository = contatoRepository;
        _logger = logger;
        _factory = new ConnectionFactory()
        {
            HostName = config["RabbitMQ:Hostname"] ?? string.Empty,
            UserName = config["RabbitMQ:Username"] ?? string.Empty,
            Password = config["RabbitMQ:Password"] ?? string.Empty,
        };
        _queueName = config["RabbitMQ:QueueName"] ?? string.Empty;

        Setup().Wait();
    }
    
    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Finalizando o Consumer...");

        _channel?.Dispose();
        _connection?.Dispose();

        _logger.LogInformation("Conexão fechada.");

        await base.StopAsync(stoppingToken);
    }

    private async Task Setup()
    {
        _connection = await _factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();

        await _channel.QueueDeclareAsync(
            queue: _queueName,
            durable: true,
            exclusive: false,
            autoDelete: false
        );
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new AsyncEventingBasicConsumer(_channel);
        _logger.LogInformation("Consumer iniciado. Aguardando mensagens...");

        consumer.ReceivedAsync += Contato_Recebido;

        await _channel.BasicConsumeAsync(
                queue: _queueName,
                autoAck: false,
                consumer: consumer, 
                cancellationToken: stoppingToken);

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    private async Task Contato_Recebido(object sender, BasicDeliverEventArgs args)
    {
        var body = args.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        var contato = JsonSerializer.Deserialize<Contato>(message);

        if (contato != null) 
            await _contatoRepository.CreateAsync(contato);

        await _channel.BasicAckAsync(deliveryTag: args.DeliveryTag, multiple: false, cancellationToken: args.CancellationToken);

        await Task.CompletedTask;
    }
}