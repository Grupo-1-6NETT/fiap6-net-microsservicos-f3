using ContatoDb.Core.Models;

namespace Domain;

public static class AdicionarContatoDtoExtensions
{
    public static Contato ToContato(this AdicionarContatoDto dto)
    {
        return new Contato()
        {
            Nome = dto.Nome,
            Telefone = dto.Telefone,
            DDD = dto.DDD,
            Email = dto.Email
        };
    }
}