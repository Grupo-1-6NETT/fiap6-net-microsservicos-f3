namespace Domain;

public class AtualizarContatoDto
{
    public Guid ContatoId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string DDD { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}