using Cadastro.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cadastro.Infrastructure.Context;
public class CadastroDbContext : DbContext
{
    public DbSet<Contato> Contatos { get; set; }
    public CadastroDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Contato>()
            .HasKey(c => c.Id);
    }


}
