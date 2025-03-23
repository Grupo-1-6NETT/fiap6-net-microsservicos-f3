using ContatoDb.Core.Models;

namespace ContatoDb.Core.Interfaces;

public interface IContatoRepository
{    
    Task CreateAsync(Contato contato);
    Task<Contato?> GetByIdAsync(Guid id);
    Task<IEnumerable<Contato>> GetAllAsync(int? pageIndex, int? pageSize);
    Task<IEnumerable<Contato>> GetByDddAsync(string ddd, int? pageIndex, int? pageSize);
    void Update(Contato contato);
    Task DeleteAsync(Guid id);
}