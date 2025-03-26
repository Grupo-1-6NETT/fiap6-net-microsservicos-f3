using ContatoDb.Core.Models;

namespace ContatoDb.Core.Interfaces;

public interface IContatoRepository
{    
    Task CreateAsync(Contato contato);
    Task<Contato?> GetByIdAsync(Guid id);    
    void Update(Contato contato);
    Task DeleteAsync(Guid id);
}