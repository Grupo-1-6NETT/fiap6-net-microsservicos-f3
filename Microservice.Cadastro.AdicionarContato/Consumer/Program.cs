using Consumer;
using ContatoDb.Core;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

var sqliteConnString = builder.Configuration.GetSection("SQLiteConnection").Value;

builder.Services.AddContatoDbCoreServices(sqliteConnString);

var host = builder.Build();
host.Run();
