using ContatoDb.Core.Interfaces;
using Domain;
using MassTransit;

namespace Consumer.Eventos;

public class AtualizarContatoConsumidor(IContatoRepository contatoRepository) : IConsumer<AtualizarContatoDto>
{
    public Task Consume(ConsumeContext<AtualizarContatoDto> context)
    {        
        contatoRepository.Update(context.Message.ToContato());
        
        return Task.CompletedTask;
    }
}

