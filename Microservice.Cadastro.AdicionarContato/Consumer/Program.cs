using Consumer;
using ContatoDb.Core;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddInfrastructureServices(builder.Configuration);

var host = builder.Build();
host.Run();
