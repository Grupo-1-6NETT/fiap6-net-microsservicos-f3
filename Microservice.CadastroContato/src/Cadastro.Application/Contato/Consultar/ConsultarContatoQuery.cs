using MediatR;

namespace Cadastro.Application.Contato.Consultar;
public class ConsultarContatoQuery : IRequest<IEnumerable<Domain.Models.Contato>>
{
    public string? DDD { get; set; }
    public int? PageIndex { get; set; }
    public int? PageSize { get; set; }
}
