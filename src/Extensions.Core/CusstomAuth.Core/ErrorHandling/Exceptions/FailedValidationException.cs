namespace CusstomAuth.Core.ErrorHandling.Exceptions;

public class FailedValidationException : HttpResponseException
{
    public FailedValidationException() : base(400, "Validation failed")
    {

    }

    public FailedValidationException(string error) : base(400, error)
    {

    }

    public FailedValidationException(Dictionary<string, List<string>> validations) : this()
    {
        ValidationErrors = validations;
    }

    public Dictionary<string, List<string>> ValidationErrors;
}
