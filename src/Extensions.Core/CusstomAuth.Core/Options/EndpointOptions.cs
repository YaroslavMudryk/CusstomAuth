namespace CusstomAuth.Core.Options;

public class EndpointOptions
{
    public string Endpoint { get; set; } = default!;
    public bool IsAvailable { get; set; }
    public string HttpMethod { get; set; } = default!;
    public bool IsSecure { get; set; }
}
