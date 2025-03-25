using Cadastro.Auth.Infra.Context;
using Cadastro.Auth.Infra.Repository;
using Cadastro.Auth.Domain.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Cadastro.Auth.Domain.IToken;
using Cadastro.Auth.Infra.Services;

namespace Cadastro.Auth.Infra
{
    public static class ServiceExtensions
    {
        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Substitui SQLite por InMemory Database
            services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase("InMemoryDb"));

            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddSingleton<ITokenService, TokenService>();
        }
    }
}
