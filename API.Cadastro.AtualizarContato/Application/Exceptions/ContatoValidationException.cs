namespace Application.Exceptions;

public class ContatoValidationException(List<string> errors) : Exception("Erro(s) de validação encontrados.")
{
    public List<string> Errors { get; } = errors;
}
