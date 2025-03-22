using Application.Contato;
using Application.Services;
using Moq;

namespace UnitTests.Application;

public class AtualizarContatoHandlerTests
{
    private readonly Mock<IRabbitMQService> _rabbitMqServiceMock;
    private readonly AtualizarContatoHandler _handler;    

    public AtualizarContatoHandlerTests()
    {
        _rabbitMqServiceMock = new Mock<IRabbitMQService>();
        _handler = new (_rabbitMqServiceMock.Object);
    }

    [Fact]
    public async Task Handle_DeveRetornarOk()
    {
        var id = Guid.NewGuid();
        var command = new AtualizarContatoCommand(id)
        {
            Email = "test@test.com",
            Telefone = "999999999",
            DDD = "99",
            Nome = "test"
        };

        var result = await _handler.Handle(command, CancellationToken.None);

        _rabbitMqServiceMock.Verify(x => x.PublicarMensagem(It.IsAny<AtualizarContatoCommand>()), Times.Once);     
        
        _rabbitMqServiceMock.Invocations.Clear();
    }
}