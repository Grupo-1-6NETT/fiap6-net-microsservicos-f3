using Cadastro.Auth.Domain.Models;
using Cadastro.Auth.Domain.IRepository;
using MediatR;

namespace Cadastro.Auth.Application;

public class AdicionarUsuarioCommandHandler : IRequestHandler<AdicionarUsuarioCommand, Guid>
{
    private readonly IUsuarioRepository _usuarioRepository;

    public AdicionarUsuarioCommandHandler(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<Guid> Handle(AdicionarUsuarioCommand command, CancellationToken cancellationToken)
    {
        command.Validate();

        var usuario = new Usuario
        {
            Nome = command.Nome,
            Permissao = command.Permissao.Value //Validate deve garantir que permissão não é null aqui
        };

        await _usuarioRepository.AddUserAsync(usuario, command.Senha);

        return usuario.Id;
    }
}
