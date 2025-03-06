namespace CusstomAuth.Core.ErrorHandling.Exceptions;

public class AppNotActivatedException : HttpResponseException
{
    public AppNotActivatedException() : this("App not active or expired")
    {

    }

    public AppNotActivatedException(string message) : base(404, message)
    {

    }
}
