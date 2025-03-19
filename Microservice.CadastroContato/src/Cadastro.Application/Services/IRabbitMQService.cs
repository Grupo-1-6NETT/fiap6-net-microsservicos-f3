namespace Cadastro.Application.Services;

public interface IRabbitMQService
{
    Task PublicarMensagem<T>(T mensagem, string routingKey);
}
