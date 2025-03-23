using Domain;

namespace Application.Services;

public interface IRabbitMQService
{
    Task PublicarMensagem(AtualizarContatoDto mensagem);
}
