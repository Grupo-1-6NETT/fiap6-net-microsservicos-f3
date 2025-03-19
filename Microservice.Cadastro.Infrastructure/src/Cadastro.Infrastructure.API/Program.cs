using Cadastro.Infrastructure.Entities;
using Cadastro.Infrastructure.Extensions;
using Cadastro.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

//Serviços
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Endpoints
app.MapGet("/Contatos", async (IContatoRepository rep, string? ddd = null, int? pageIndex = null, int? pageSize = null) =>
{
    var contatos = await rep.GetByDDDAsync(ddd, pageIndex, pageSize);
    return contatos.Any() ? Results.Ok(contatos) : Results.NotFound();
});

app.MapPost("/Contatos", async (IContatoRepository rep, Contato request) =>
{
    var novoContato = new Contato
    {
        Nome = request.Nome,
        Telefone = request.Telefone,
        DDD = request.DDD,
        Email = request.Email        
    };

    await rep.AddContatoAsync(novoContato);

    return Results.Created("", novoContato);
});

app.MapDelete("/Contatos/{id:guid}", async (IContatoRepository rep, Guid id) =>
{
    await rep.DeleteContatoAsync(id);
    return Results.NoContent();
});

app.Run();
