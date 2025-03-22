using ContatoDb.Core.Interfaces;
using ContatoDb.Core.Models;
using ContatoDb.Core.Repository;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Consumer.Eventos;

public class CriarContato(IContatoRepository contatoRepository)
{
    public async Task Contato_Recebido(object sender, BasicDeliverEventArgs args)
    {
        var body = args.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        var contato = JsonSerializer.Deserialize<Contato>(message);

        if (contato != null)
            await contatoRepository.CreateAsync(contato);
    }
}

