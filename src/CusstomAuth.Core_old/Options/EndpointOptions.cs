namespace CusstomAuth.Core.Options;

public class EndpointOptions
{
    public string Endpoint { get; set; }
    public bool IsAvailable { get; set; }
    public string HttpMethod { get; set; }
    public bool IsSecure { get; set; }
}
