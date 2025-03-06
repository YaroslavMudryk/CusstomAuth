using CusstomAuth.Core.Constants;
using CusstomAuth.Core.Features.Sessions.Services;
using CusstomAuth.Core.Handlers;
using CusstomAuth.Core.Options;
using CusstomAuth.Core.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace CusstomAuth.Core.Features.Sessions;

public class CloseSessionsEndpointHandler : IEndpointHandler
{
    public CloseSessionsEndpointHandler()
    {

    }

    public CloseSessionsEndpointHandler(string action, EndpointOptions endpoint)
    {
        Action = action;
        Endpoint = endpoint;
    }

    public string Action { get; } = HttpActions.CloseSessionsAction;

    public EndpointOptions Endpoint { get; }

    public IEndpointHandler CreateFromOptions(Dictionary<string, EndpointOptions> options)
    {
        var currentOption = options[Action];
        return new CloseSessionsEndpointHandler(Action, currentOption);
    }

    public async Task HandleAsync(HttpContext context)
    {
        string[] sessionIds = context.Request.Query["sessionIds"];

        var sessionService = context.RequestServices.GetRequiredService<ISessionService>();

        var closedSessions = await sessionService.CloseSessionsByIdsAsync(sessionIds);

        if (sessionIds.Count() != closedSessions)
            await context.Response.WriteAsJsonAsync(ApiResponse.SuccessWithWarning(200, closedSessions, $"Please note that you wanted to close {sessionIds.Count()} sessions, but for some reason we were only able to close {closedSessions}"));
        else
            await context.Response.WriteAsJsonAsync(ApiResponse.Success(200, closedSessions));
    }
}
