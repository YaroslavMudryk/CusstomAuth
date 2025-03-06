namespace CusstomAuth.Core.ErrorHandling.Exceptions;

public class InternalServerException : HttpResponseException
{
    public InternalServerException() : this("Internal server error")
    {

    }

    public InternalServerException(string message) : base(500, message)
    {

    }
}
