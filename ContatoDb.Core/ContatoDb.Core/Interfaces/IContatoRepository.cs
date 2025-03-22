using ContatoDb.Core.Models;

namespace ContatoDb.Core.Interfaces;

public interface IContatoRepository
{    
    Task<Contato?> GetByIdAsync(Guid id);
    Task CreateAsync(Contato contato);
}