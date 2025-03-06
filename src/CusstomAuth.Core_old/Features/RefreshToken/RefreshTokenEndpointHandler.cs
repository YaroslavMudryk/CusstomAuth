using CusstomAuth.Core.Constants;
using CusstomAuth.Core.Features.RefreshToken.Services;
using CusstomAuth.Core.Handlers;
using CusstomAuth.Core.Options;
using CusstomAuth.Core.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace CusstomAuth.Core.Features.RefreshToken;

public class RefreshTokenEndpointHandler : IEndpointHandler
{
    public RefreshTokenEndpointHandler()
    {

    }

    public RefreshTokenEndpointHandler(string action, EndpointOptions endpoint)
    {
        Action = action;
        Endpoint = endpoint;
    }

    public string Action { get; } = HttpActions.RefreshTokenAction;

    public EndpointOptions Endpoint { get; }

    public IEndpointHandler CreateFromOptions(Dictionary<string, EndpointOptions> options)
    {
        var currentOption = options[Action];
        return new RefreshTokenEndpointHandler(Action, currentOption);
    }

    public async Task HandleAsync(HttpContext context)
    {
        string token = context.Request.Query["token"];

        var authService = context.RequestServices.GetRequiredService<IRefreshTokenService>();

        var result = await authService.RefreshTokenAsync(token);

        await context.Response.WriteAsJsonAsync(ApiResponse.Success(200, result));
    }
}
