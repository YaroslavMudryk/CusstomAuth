namespace CusstomAuth.Core.ErrorHandling.Exceptions;

public class BadRequestException : HttpResponseException
{
    public BadRequestException() : this("BadRequest")
    {

    }

    public BadRequestException(string message) : base(400, message)
    {

    }
}
