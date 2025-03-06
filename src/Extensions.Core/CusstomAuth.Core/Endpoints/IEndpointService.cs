using CusstomAuth.Core.ErrorHandling.Exceptions;
using CusstomAuth.Core.Options;
using CusstomAuth.Core.SessionsManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace CusstomAuth.Core.Endpoints;

public interface IEndpointService
{
    Task CheckHandlerAuthorizationAsync(EndpointOptions endpointOptions);
}

public class EndpointService(
    IHttpContextAccessor httpContextAccessor,
    ISessionService sessionSevice,
    ICurrentContext currentContext,
    IOptions<IdentityOptions> options) : IEndpointService
{
    public async Task CheckHandlerAuthorizationAsync(EndpointOptions endpoint)
    {
        if (!endpoint.IsSecure)
            return;

        var sessionId = currentContext.User.SessionId;

        if (!httpContextAccessor.HttpContext.User.Identity!.IsAuthenticated)
            throw new UnauthorizedException();

        if (options.Value.Token.UseSessionManager)
            if (!await sessionSevice.IsActiveSessionAsync(sessionId))
                throw new UnauthorizedException();
    }
}