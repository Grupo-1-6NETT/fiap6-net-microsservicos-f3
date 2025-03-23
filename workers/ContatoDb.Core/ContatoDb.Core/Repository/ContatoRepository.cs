using ContatoDb.Core.Data;
using ContatoDb.Core.Interfaces;
using ContatoDb.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ContatoDb.Core.Repository;

public class ContatoRepository(AppDbContext context) : IContatoRepository
{
    public async Task CreateAsync(Contato contato)
    {
        await context.Set<Contato>().AddAsync(contato);
        await context.SaveChangesAsync();
    }
    
    public async Task<Contato?> GetByIdAsync(Guid id)
    {
        return await context.Set<Contato>().FindAsync(id);
    }

    public async Task<IEnumerable<Contato>> GetAllAsync(int? pageIndex, int? pageSize)
    {
        var query = context.Set<Contato>()
            .AsNoTracking()            
            .AsQueryable();

        if (pageIndex is null || pageSize is null)
            return await query.ToListAsync();

        return await query
            .Skip((pageIndex.Value - 1) * pageSize.Value)
            .Take(pageSize.Value)
            .ToListAsync();
    }

    public async Task<IEnumerable<Contato>> GetByDddAsync(string ddd, int? pageIndex, int? pageSize)
    {
        var query = context.Contatos
            .AsNoTracking()
            .Where(c => c.DDD == ddd)
            .OrderBy(c => c.Nome)
            .AsQueryable();

        if (pageIndex is null || pageSize is null)
            return await query.ToListAsync();

        return await query
            .Skip((pageIndex.Value - 1) * pageSize.Value)
            .Take(pageSize.Value)
            .ToListAsync();
    }
    
    public void Update(Contato contato)
    {
        context.Set<Contato>().Update(contato);
        context.SaveChanges();
    }
    
    public async Task DeleteAsync(Guid id)
    {
        var contato = await GetByIdAsync(id);
        if (contato is not null)
        {
            context.Set<Contato>().Remove(contato);
            await context.SaveChangesAsync();
        }
    }
}