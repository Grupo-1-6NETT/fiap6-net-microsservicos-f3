using Application.Contato;
using Application.Services;
using Application.Exceptions;
using Domain.Models;
using FluentValidation;
using Moq;

namespace UnitTest.Application.ContatoTests;

public class AdicionarContatoHandlerTests
{
    private readonly Mock<IRabbitMQService> _rabbitMqServiceMock;
    private readonly AdicionarContatoHandler _handler;    

    public AdicionarContatoHandlerTests()
    {
        _rabbitMqServiceMock = new Mock<IRabbitMQService>();
        _handler = new (_rabbitMqServiceMock.Object);
    }

    [Fact]
    public async Task Handle_InformadosDadosValidos_DeveRetornarOk()
    {
        var command = new AdicionarContatoCommand
        {
            Nome = "Batman",
            Telefone = "999999999",
            DDD = "11",
            Email = "batman@gotham.com"
        };

        var result = await _handler.Handle(command, CancellationToken.None);

        _rabbitMqServiceMock.Verify(x => x.PublicarMensagem(It.IsAny<Contato>()), Times.Once);     
        
        _rabbitMqServiceMock.Invocations.Clear();
    }

    [Fact]
    public async Task Handle_InformadosDadosInvalidos_ValidationException()
    {
        var command = new AdicionarContatoCommand
        {
            Nome = "Batman",
            Telefone = "99",
            DDD = "11",
            Email = "batman@gotham.com"
        };

        await Assert.ThrowsAsync<ContatoValidationException>(async () => await _handler.Handle(command, CancellationToken.None));
    }
}