using CusstomAuth.Core.Constants;
using CusstomAuth.Core.Extensions;
using CusstomAuth.Core.Features.Mfa.Services;
using CusstomAuth.Core.Handlers;
using CusstomAuth.Core.Options;
using CusstomAuth.Core.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace CusstomAuth.Core.Features.Mfa;

public class TurnOnMfaEndpointHandler : IEndpointHandler
{
    public TurnOnMfaEndpointHandler()
    {

    }

    public TurnOnMfaEndpointHandler(string action, EndpointOptions endpoint)
    {
        Action = action;
        Endpoint = endpoint;
    }

    public string Action { get; } = HttpActions.TurnOnMfaAction;

    public EndpointOptions Endpoint { get; }

    public IEndpointHandler CreateFromOptions(Dictionary<string, EndpointOptions> options)
    {
        var currentOption = options[Action];
        return new TurnOnMfaEndpointHandler(Action, currentOption);
    }

    public async Task HandleAsync(HttpContext context)
    {
        string code = context.Request.Query["code"];

        var mfaService = context.RequestServices.GetRequiredService<IMfaService>();

        var result = await mfaService.EnableMfaAsync(code);

        await context.Response.WriteAsJsonAsync(ApiResponse.Success(200, result), Settings.Api);
    }
}
