using ContatoDb.Core.Models;

namespace ContatoDb.Core.Data;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Contato> Contatos { get; set; }
}