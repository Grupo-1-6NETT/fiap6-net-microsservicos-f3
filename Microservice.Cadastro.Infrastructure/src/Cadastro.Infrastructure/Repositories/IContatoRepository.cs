using Cadastro.Infrastructure.Entities;

namespace Cadastro.Infrastructure.Repositories;
public interface IContatoRepository
{
    Task<IEnumerable<Contato>> GetByDDDAsync(string? ddd, int? pageIndex, int? pageSize);
    Task<Contato> GetById(Guid id);
    Task<bool> Exists(Guid id);
    Task AddContatoAsync(Contato contato);
    Task UpdateContatoAsync(Contato contato);
    Task DeleteContatoAsync(Guid id);
}
