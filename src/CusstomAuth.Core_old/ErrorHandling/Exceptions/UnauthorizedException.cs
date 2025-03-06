namespace CusstomAuth.Core.ErrorHandling.Exceptions;

public class UnauthorizedException : HttpResponseException
{
    public UnauthorizedException() : this("Unauthorized")
    {

    }

    public UnauthorizedException(string message) : base(401, message)
    {

    }
}
