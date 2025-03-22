using ContatoDb.Core.Data;
using ContatoDb.Core.Interfaces;
using ContatoDb.Core.Models;

namespace ContatoDb.Core.Repository;

public class ContatoRepository(AppDbContext context) : IContatoRepository
{
    public async Task<Contato?> GetByIdAsync(Guid id)
    {
        return await context.Set<Contato>().FindAsync(id);
    }

    public async Task CreateAsync(Contato contato)
    {
        await context.Set<Contato>().AddAsync(contato);
        await context.SaveChangesAsync();
    }
}