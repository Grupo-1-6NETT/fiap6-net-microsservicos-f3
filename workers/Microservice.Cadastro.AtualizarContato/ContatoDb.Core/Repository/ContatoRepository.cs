using ContatoDb.Core.Data;
using ContatoDb.Core.Interfaces;
using ContatoDb.Core.Models;

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
    
    public void Update(Contato contato)
    {
        var contatoDB = context.Contatos.FirstOrDefault(c => c.Id == contato.Id);
        if (contatoDB == null)
            return;

        if (!string.IsNullOrWhiteSpace(contato.Nome)) contatoDB.Nome = contato.Nome;
        if (!string.IsNullOrWhiteSpace(contato.Telefone)) contatoDB.Telefone = contato.Telefone;
        if (!string.IsNullOrWhiteSpace(contato.DDD)) contatoDB.DDD = contato.DDD;
        if (!string.IsNullOrWhiteSpace(contato.Email)) contatoDB.Email = contato.Email;

        context.Set<Contato>().Update(contatoDB);
        context.SaveChanges();
    }
    
    public async Task DeleteAsync(Guid id)
    {
        var contato = context.Contatos.FirstOrDefault(c => c.Id == id);
        if (contato is not null)
        {
            context.Set<Contato>().Remove(contato);
            await context.SaveChangesAsync();
        }
    }
}