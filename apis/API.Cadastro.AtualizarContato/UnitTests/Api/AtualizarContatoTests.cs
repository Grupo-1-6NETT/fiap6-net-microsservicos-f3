using API.Controllers;
using Application.Contato;
using Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTests.Api;

public class AtualizarContatoTests
{
    private readonly Mock<IMediator> _mediatorMock;    
    private readonly AtualizarContatoController _controller;

    public AtualizarContatoTests()
    {
        _mediatorMock = new Mock<IMediator>();     
        _controller = new (_mediatorMock.Object);

    }

    [Fact]
    public async Task Patch_DeverRetornarOk()
    {
        var contatoId = Guid.NewGuid();
        var command = new AtualizarContatoCommand(contatoId)
        {
            Email = "test@test.com",
            Telefone = "999999999",
            DDD = "99",
            Nome = "test"
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<AtualizarContatoCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(contatoId);

        var result = _controller.Patch(command);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var resultValue = await ((Task<Guid>)okResult.Value!)!;
        Assert.Equal(contatoId, resultValue);
    }
    
    [Fact]
    public void Patch_DadosInvalidos_DeveRetornarBadRequest()
    {
        var contatoId = Guid.NewGuid();
        var command = new AtualizarContatoCommand(contatoId)
        {            
            DDD = "1111",
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<AtualizarContatoCommand>(), It.IsAny<CancellationToken>()))
            .Throws(new ContatoValidationException(new List<string>()));

        var result = _controller.Patch(command);
        
        Assert.IsType<BadRequestObjectResult>(result);
    }
}