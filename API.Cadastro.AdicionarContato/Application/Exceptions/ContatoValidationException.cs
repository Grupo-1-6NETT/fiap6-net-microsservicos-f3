namespace Application.Exceptions;

public class ContatoValidationException : Exception
{
    public List<string> Errors { get; }

    public ContatoValidationException(List<string> errors)
        : base("Erro(s) de validação encontrados.")
    {
        Errors = errors;
    }
}
