using Cadastro.Auth.Infra.Context;
using Cadastro.Auth.Infra.Repository;
using Cadastro.Auth.Domain.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Cadastro.Auth.Domain.IToken;
using Cadastro.Auth.Infra.Services;
using System.Text;

namespace Cadastro.Auth.Infra
{
    public static class ServiceExtensions
    {
        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Verifica se a string de conexão está presente
            var connectionString = configuration.GetConnectionString("SQLiteConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("Connection string 'SQLiteConnection' is missing.");
            }

            // Adiciona o contexto do banco de dados
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(connectionString));

            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddSingleton<ITokenService, TokenService>();
        }
    }
}
