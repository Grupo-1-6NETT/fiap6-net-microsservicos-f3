using ContatoDb.Core.Data;
using ContatoDb.Core.Interfaces;
using ContatoDb.Core.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ContatoDb.Core;

public static class InfrastructureExtensions
{
    public static void AddContatoDbCoreServices(this IServiceCollection services, string connString)
    {
        // Verifica se a string de conexão está presente        
        if (string.IsNullOrEmpty(connString))
        {
            throw new ArgumentNullException("Connection string 'SQLiteConnection' is missing.");
        }

        // Adiciona o contexto do banco de dados
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(connString));

        services.AddScoped<IContatoRepository, ContatoRepository>();
    }
}