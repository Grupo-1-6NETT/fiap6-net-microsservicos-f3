using Cadastro.Infrastructure.Context;
using Cadastro.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cadastro.Infrastructure.Repositories;
public class ContatoRepository : IContatoRepository
{
    private readonly CadastroDbContext _context;

    public ContatoRepository(CadastroDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Contato>> GetByDDDAsync(string? ddd, int? pageIndex, int? pageSize)
    {
        var query = _context.Contatos
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrEmpty(ddd))
        {
            query = query.Where(c => c.DDD == ddd);
        }

        query = query.OrderBy(c => c.Nome);

        if (pageIndex is null || pageSize is null)
            return await query.Take(50).ToListAsync();

        return await query
            .Skip((pageIndex.Value - 1) * pageSize.Value)
            .Take(pageSize.Value)
            .ToListAsync();
    }
    public async Task<Contato?> GetById(Guid id)
    {
        return await _context.Contatos.FindAsync(id);       
    }
    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Contatos.AnyAsync(c => c.Id == id);
    }

    public async Task AddContatoAsync(Contato contato)
    {
        await _context.Contatos.AddAsync(contato);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateContatoAsync(Contato contato)
    {
        _context.Contatos.Update(contato);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteContatoAsync(Guid id)
    {
        var entity = await _context.Contatos.FindAsync(id);
        if (entity is not null)
        {
            _context.Contatos.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

}
