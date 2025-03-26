using System.ComponentModel.DataAnnotations;

namespace ContatoDb.Core.Models;

public class Contato
{
    [Key]
    public Guid Id { get; set; }

    public string Nome { get; set; } = String.Empty;

    public string Telefone { get; set; } = String.Empty;

    public string DDD { get; set; } = String.Empty;

    public string Email { get; set; } = String.Empty;
}