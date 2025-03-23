using Application.Contato;
using FluentValidation;

namespace Application.Validators;

public class ModelContatoValidator : AbstractValidator<AtualizarContatoCommand>
{
    public ModelContatoValidator()
    {
        RuleFor(c => c.Nome)
                .MaximumLength(100).WithMessage("O nome pode ter no máximo 100 caracteres")
                .When(c => !string.IsNullOrWhiteSpace(c.Nome));

        RuleFor(c => c.Telefone)
            .Matches(@"^\d{8,9}$").WithMessage("Telefone deve conter entre 8 e 9 dígitos")
            .When(c => !string.IsNullOrWhiteSpace(c.Telefone));

        RuleFor(c => c.DDD)
            .Length(2).WithMessage("DDD deve ter 2 dígitos")
            .Must(ddd => int.TryParse(ddd, out int result) && result >= 11 && result <= 99).WithMessage("DDD deve estar entre 11 e 99")
            .When(c => !string.IsNullOrWhiteSpace(c.DDD));

        RuleFor(c => c.Email)
            .EmailAddress().WithMessage("Formato de email inválido")
            .When(c => !string.IsNullOrWhiteSpace(c.Email)); ;
    }
}
