using Consumer;
using Consumer.Eventos;
using ContatoDb.Core;
using MassTransit;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

var configuration = builder.Configuration;

var sqliteConnString = configuration.GetSection("SQLiteConnection").Value;
builder.Services.AddContatoDbCoreServices(sqliteConnString);


var fila = configuration.GetSection("RabbitMQ")["QueueName"] ?? string.Empty;
var servidor = configuration.GetSection("RabbitMQ")["Hostname"] ?? string.Empty;
var usuario = configuration.GetSection("RabbitMQ")["Username"] ?? string.Empty;
var senha = configuration.GetSection("RabbitMQ")["Password"] ?? string.Empty;

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(servidor, "/", h =>
        {
            h.Username(usuario);
            h.Password(senha);
        });

        cfg.ReceiveEndpoint(fila, e =>
        {
            e.ConfigureConsumer<AtualizarContatoConsumidor>(context);
        });

        cfg.ConfigureEndpoints(context);
    });

    x.AddConsumer<AtualizarContatoConsumidor>();
});



var host = builder.Build();
host.Run();
