using API.Controllers;
using Application.Contato;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTests.Api;

public class RemoverContatoTests
{
    private readonly Mock<IMediator> _mediatorMock;    
    private readonly RemoverContatoController _controller;

    public RemoverContatoTests()
    {
        _mediatorMock = new Mock<IMediator>();     
        _controller = new (_mediatorMock.Object);

    }

    [Fact]
    public async Task Delete_DeverRetornarOk()
    {
        var contatoId = Guid.NewGuid();
        var command = new RemoverContatoCommand(contatoId);

        _mediatorMock.Setup(m => m.Send(It.IsAny<RemoverContatoCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(contatoId);

        var result = await _controller.Delete(contatoId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var resultValue = ((string)okResult.Value!)!;
        Assert.Equal($"Contato com {contatoId} enviado para remoção.", okResult.Value);
    }
}