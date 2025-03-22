using Core.Data;
using Core.Repository;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core;

public static class InfrastructureExtensions
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

        services.AddScoped<IContatoRepository, ContatoRepository>();
    }
}