using CusstomAuth.Core.Constants;
using CusstomAuth.Core.Endpoints;
using CusstomAuth.Core.Extensions;
using CusstomAuth.Core.Initialization;
using CusstomAuth.Core.Options;
using CusstomAuth.Core.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace CusstomAuth.Core.Features.Init;

public class SeedEndpointHandler : IEndpointHandler
{
    public SeedEndpointHandler()
    {

    }

    public SeedEndpointHandler(string action, EndpointOptions endpoint)
    {
        Action = action;
        Endpoint = endpoint;
    }

    public string Action { get; } = HttpActions.Seed;

    public EndpointOptions Endpoint { get; }

    public IEndpointHandler CreateFromOptions(Dictionary<string, EndpointOptions> options)
    {
        var currentOption = options[Action];
        return new SeedEndpointHandler(Action, currentOption);
    }

    public async Task HandleAsync(HttpContext context)
    {
        var dbInitializer = context.RequestServices.GetRequiredService<IDbInitialization>();
        var result = await dbInitializer.InitializeAsync();

        await context.Response.WriteAsJsonAsync(ApiResponse.Success(200, result));
    }
}
