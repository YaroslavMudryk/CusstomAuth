using CusstomAuth.Core.Constants;
using CusstomAuth.Core.Features.Sessions.Services;
using CusstomAuth.Core.Handlers;
using CusstomAuth.Core.Options;
using CusstomAuth.Core.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace CusstomAuth.Core.Features.Sessions;

public class SessionsEndpointHandler : IEndpointHandler
{
    public SessionsEndpointHandler()
    {

    }

    public SessionsEndpointHandler(string action, EndpointOptions endpoint)
    {
        Action = action;
        Endpoint = endpoint;
    }

    public string Action { get; } = HttpActions.SessionsAction;

    public EndpointOptions Endpoint { get; }

    public IEndpointHandler CreateFromOptions(Dictionary<string, EndpointOptions> options)
    {
        var currentOption = options[Action];
        return new SessionsEndpointHandler(Action, currentOption);
    }

    public async Task HandleAsync(HttpContext context)
    {
        var authService = context.RequestServices.GetRequiredService<ISessionService>();

        var result = await authService.GetUserSessionsAsync();

        await context.Response.WriteAsJsonAsync(ApiResponse.Success(200, result));
    }
}
