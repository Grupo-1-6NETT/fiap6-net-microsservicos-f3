using Application.Contato;
using Application.Services;
using Domain;
using Moq;

namespace UnitTests.Application;

public class RemoverContatoHandlerTests
{
    private readonly Mock<IRabbitMQService> _rabbitMqServiceMock;
    private readonly RemoverContatoHandler _handler;    

    public RemoverContatoHandlerTests()
    {
        _rabbitMqServiceMock = new Mock<IRabbitMQService>();
        _handler = new (_rabbitMqServiceMock.Object);
    }

    [Fact]
    public async Task Handle_DeveRetornarOk()
    {
        var id = Guid.NewGuid();
        var command = new RemoverContatoCommand(id);

        var result = await _handler.Handle(command, CancellationToken.None);

        _rabbitMqServiceMock.Verify(x => x.PublicarMensagem(It.IsAny<RemoverContatoDto>()), Times.Once);     
        
        _rabbitMqServiceMock.Invocations.Clear();
    }
}