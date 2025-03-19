using Cadastro.Consulta.Consumer.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Cadastro.Consulta.Consumer;
public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConnectionFactory _factory;
    private IChannel _channel;
    private IConnection _connection;

    public Worker(IConfiguration config, ILogger<Worker> logger)
    {
        _logger = logger;
        _factory = new ConnectionFactory()
        {
            HostName = config["RabbitMQ:HostName"],
            UserName = config["RabbitMQ:UserName"],
            Password = config["RabbitMQ:Password"]
        };

        Setup().Wait();
    }

    private async Task Setup()
    {
        _connection = await _factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();

        await _channel.QueueDeclareAsync(
            queue: "contatos.consulta",
            durable: true,
            exclusive: false,
            autoDelete: false
        );
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new AsyncEventingBasicConsumer(_channel);
        _logger.LogInformation("Consumer iniciado. Aguardando mensagens...");


        consumer.ReceivedAsync += async (model, args) =>
        {
            var body = args.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($"Mensagem Recebida: {message}");

            await ProcessarMensagem(message);

            await _channel.BasicAckAsync(deliveryTag: args.DeliveryTag, multiple:false);

            await Task.CompletedTask;

        };

            await _channel.BasicConsumeAsync(
                    queue: "contatos.consulta",
                    autoAck: false,
                    consumer: consumer
            );

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Finalizando o Consumer...");

        _channel?.Dispose();
        _connection?.Dispose();

        _logger.LogInformation("Conexão fechada.");

        await base.StopAsync(stoppingToken);
    }

    private async Task ProcessarMensagem(string? mensagem)
    {
        var consultaRequest = JsonSerializer.Deserialize<ConsultaRequest>(mensagem);

        using var httpClient = new HttpClient();

        string apiUrl = $"https://localhost:7095/Contatos?ddd={consultaRequest.DDD}";

        if (consultaRequest.PageIndex is not null)
            apiUrl += $"&pageIndex={consultaRequest.PageIndex}";

        if (consultaRequest.PageSize is not null)
            apiUrl += $"&pageSize={consultaRequest.PageSize}";

        HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

        if (response.IsSuccessStatusCode)
        {
            var contatos = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Contatos recebidos: {contatos}");
        }
        else
        {
            Console.WriteLine("Nenhum contato encontrado.");
        }

    }
}