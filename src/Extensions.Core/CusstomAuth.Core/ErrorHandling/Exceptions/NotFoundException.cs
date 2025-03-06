namespace CusstomAuth.Core.ErrorHandling.Exceptions;

public class NotFoundException : HttpResponseException
{
    public NotFoundException() : this("NotFound")
    {

    }

    public NotFoundException(string message) : base(404, message)
    {

    }
}
