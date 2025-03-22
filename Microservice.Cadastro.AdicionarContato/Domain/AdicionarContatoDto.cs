namespace Domain;

public class AdicionarContatoDto
{
    public Guid TransportId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string DDD { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}