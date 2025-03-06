using CusstomAuth.Core.ErrorHandling.Exceptions;
using CusstomAuth.Core.Options;
using CusstomAuth.Core.Services.Auth.SessionsManager;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace CusstomAuth.Core.Services.Auth;

public class EndpointService(
    IHttpContextAccessor httpContextAccessor,
    ISessionManager sessionManager,
    ICurrentContext currentContext,
    IOptions<CusstomAuthOptions> options) : IEndpointService
{
    public async Task CheckHandlerAuthorizationAsync(EndpointOptions endpoint)
    {
        if (!endpoint.IsSecure)
            return;

        var sessionId = currentContext.User.SessionId;

        if (!httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            throw new UnauthorizedException();

        if (options.Value.Token.UseSessionManager)
            if (!await sessionManager.IsActiveSessionAsync(sessionId))
                throw new UnauthorizedException();
    }
}
