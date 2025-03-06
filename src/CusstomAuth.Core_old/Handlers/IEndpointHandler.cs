using CusstomAuth.Core.Options;
using Microsoft.AspNetCore.Http;

namespace CusstomAuth.Core.Handlers;

public interface IEndpointHandler
{
    string Action { get; }

    EndpointOptions Endpoint { get; }

    IEndpointHandler CreateFromOptions(Dictionary<string, EndpointOptions> options);

    Task HandleAsync(HttpContext context);

    virtual bool CanHandle(HttpContext context)
    {
        return context.Request.Path.Equals(Endpoint.Endpoint, StringComparison.OrdinalIgnoreCase) && context.Request.Method == Endpoint.HttpMethod && Endpoint.IsAvailable;
    }
}