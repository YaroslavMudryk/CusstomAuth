using CusstomAuth.Core.Handlers;
using CusstomAuth.Core.Options;
using CusstomAuth.Core.Services.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CusstomAuth.Core.Middlewares;

public class CusstomAuthMiddleware(RequestDelegate next, IOptions<CusstomAuthOptions> options)
{
    private IEnumerable<IEndpointHandler> _endpointHandlers;

    public async Task InvokeAsync(HttpContext context, IServiceScopeFactory serviceScopeFactory)
    {
        using var scope = serviceScopeFactory.CreateScope();
        _endpointHandlers = scope.ServiceProvider.GetRequiredService<IEnumerable<IEndpointHandler>>().Select(eh => eh.CreateFromOptions(options.Value.Endpoints));

        foreach (var handler in _endpointHandlers)
        {
            if (handler.CanHandle(context))
            {
                var authService = scope.ServiceProvider.GetRequiredService<IEndpointService>();
                await authService.CheckHandlerAuthorizationAsync(handler.Endpoint);
                await handler.HandleAsync(context);
                break;
            }
        }

        await next(context).ConfigureAwait(false);
    }
}
