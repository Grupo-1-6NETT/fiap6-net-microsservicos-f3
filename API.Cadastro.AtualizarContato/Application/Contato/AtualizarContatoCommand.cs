using Application.Exceptions;
using Application.Validators;
using MediatR;

namespace Application.Contato;

public class AtualizarContatoCommand(Guid id) : IRequest<Guid>
{
    public Guid Id { get; } = id;
    public string Nome { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string DDD { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    
    public void Validate()
    {
        var validator = new ModelContatoValidator();
        var result = validator.Validate(this);
        if (!result.IsValid)
        {
            var errors = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ContatoValidationException(errors);
        }
    }
}
