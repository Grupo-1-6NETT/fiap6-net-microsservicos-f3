using API.Controllers;
using Application.Contato;
using Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTests.Api;

public class AdicionarContatoTests
{
    private readonly Mock<IMediator> _mediatorMock;    
    private readonly AdicionarContatoController _controller;

    public AdicionarContatoTests()
    {
        _mediatorMock = new Mock<IMediator>();     
        _controller = new (_mediatorMock.Object);

    }

    [Fact]
    public async Task Post_InformadosDadosValidos_DeveRetornarCreatedResult()
    {
        var guid = Guid.Empty;

        var command = new AdicionarContatoCommand
        {
            Nome = "Felipe Dantas",
            Telefone = "999999999",
            DDD = "11",
            Email = "felipe@example.com"
        };
        _mediatorMock.Setup(m => m.Send(It.IsAny<AdicionarContatoCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(guid);

        var result = _controller.Post(command);
        var createdResult = Assert.IsType<CreatedResult>(result);
        var resultValue = await ((Task<Guid>)createdResult.Value!)!;

        Assert.Equal(guid, resultValue);
    }

    [Fact]
    public void Post_DadosInvalidos_DeveRetornarBadRequest()
    {
        var command = new AdicionarContatoCommand
        {
            Nome = "Felipe Dantas",
            Telefone = "999999999",
            DDD = "1111",
            Email = "felipe@example.com"
        };

        _mediatorMock
            .Setup(m => m.Send(It.IsAny<AdicionarContatoCommand>(), It.IsAny<CancellationToken>()))
            .Throws(new ContatoValidationException(new List<string>()));

        var result = _controller.Post(command);
        
        Assert.IsType<BadRequestObjectResult>(result);
    }
}