using Cadastro.Infrastructure.Context;
using Cadastro.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cadastro.Infrastructure.Extensions;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CadastroDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("SqliteConnectionString")));

        services.AddScoped<IContatoRepository, ContatoRepository>();

        return services;
    }
}
