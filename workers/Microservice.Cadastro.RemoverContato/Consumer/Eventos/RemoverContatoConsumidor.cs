using ContatoDb.Core.Interfaces;
using Domain;
using MassTransit;

namespace Consumer.Eventos;

public class RemoverContatoConsumidor(IContatoRepository contatoRepository) : IConsumer<RemoverContatoDto>
{
    public Task Consume(ConsumeContext<RemoverContatoDto> context)
    {        
        contatoRepository.DeleteAsync(context.Message.ContatoId);
        return Task.CompletedTask;
    }
}

