using ContatoDb.Core.Data;
using ContatoDb.Core.Interfaces;
using ContatoDb.Core.Repository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ContatoDb.Core;

public static class InfrastructureExtensions
{
    public static void UseContatoDBSqLite(this IServiceCollection services, string connString)
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

    public static void UseContatoDBSqlServer(this IServiceCollection services, string connString)
    {
        // Verifica se a string de conexão está presente        
        if (string.IsNullOrEmpty(connString))
        {
            throw new ArgumentNullException("Connection string 'SQLiteConnection' is missing.");
        }

        // Adiciona o contexto do banco de dados
        services.AddDbContext<AppDbContext>(options =>
        {
            var conn = new SqlConnection(connString);
            options.UseSqlServer(conn);
        });

        services.AddScoped<IContatoRepository, ContatoRepository>();
    }
}