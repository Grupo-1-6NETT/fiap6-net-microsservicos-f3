using Core.Data;
using Domain.Interfaces;
using Domain.Models;

namespace Core.Repository;

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