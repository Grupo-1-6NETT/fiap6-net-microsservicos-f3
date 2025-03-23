using Cadastro.Auth.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Cadastro.Auth.Infra.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
      
    }
}
