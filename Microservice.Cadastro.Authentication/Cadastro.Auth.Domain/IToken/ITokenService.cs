using Cadastro.Auth.Domain.Models;

namespace Cadastro.Auth.Domain.IToken
{
    public interface ITokenService
    {
        string GetToken(Usuario usuario);
    }
}
