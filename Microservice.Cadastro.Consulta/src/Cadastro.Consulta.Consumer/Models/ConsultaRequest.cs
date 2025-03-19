namespace Cadastro.Consulta.Consumer.Models;
public class ConsultaRequest
{
    public string? DDD { get; set; }
    public int? PageIndex { get; set; }
    public int? PageSize { get; set; }
}
