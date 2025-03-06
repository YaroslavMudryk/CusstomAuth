namespace CusstomAuth.Core.ErrorHandling.Exceptions;

public class HttpResponseException : Exception
{
    public HttpResponseException()
    {

    }

    public HttpResponseException(int statusCode, string error) : this()
    {
        StatusCode = statusCode;
        Error = error;
    }

    public int StatusCode { get; set; }
    public string Error { get; set; }
    public string CorrelationId { get; set; }
}
