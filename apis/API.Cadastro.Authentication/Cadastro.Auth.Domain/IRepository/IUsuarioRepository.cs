using Cadastro.Auth.Domain.Models;

namespace Cadastro.Auth.Domain.IRepository;

public interface IUsuarioRepository : IRepository<Usuario>
{
    Task AddUserAsync(Usuario usuario, string senha);
    Task<Usuario?> GetUserAsync(string nome, string senha);
}
