using ContatoDb.Core.Models;

namespace Domain;

public static class AtualizarContatoDtoExtensions
{
    public static Contato ToContato(this AtualizarContatoDto dto)
    {
        return new Contato()
        {
            Id = dto.ContatoId,
            Nome = dto.Nome,
            Telefone = dto.Telefone,
            DDD = dto.DDD,
            Email = dto.Email
        };
    }
}