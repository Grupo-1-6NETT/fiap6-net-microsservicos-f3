using Domain.Models;

namespace Domain.Interfaces;

public interface IContatoRepository
{    
    Task<Contato?> GetByIdAsync(Guid id);
    Task CreateAsync(Contato contato);
}