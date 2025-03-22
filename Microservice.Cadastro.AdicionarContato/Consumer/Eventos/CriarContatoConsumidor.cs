using ContatoDb.Core.Interfaces;
using Domain;
using MassTransit;

namespace Consumer.Eventos;

public class CriarContatoConsumidor(IContatoRepository contatoRepository) : IConsumer<AdicionarContatoDto>
{
    public Task Consume(ConsumeContext<AdicionarContatoDto> context)
    {        
        contatoRepository.CreateAsync(context.Message.ToContato());
        
        return Task.CompletedTask;
    }
}

