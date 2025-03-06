using CusstomAuth.Core.Options;

namespace CusstomAuth.Core.Services.Auth;

public interface IEndpointService
{
    Task CheckHandlerAuthorizationAsync(EndpointOptions endpointOptions);
}
