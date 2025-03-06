using CusstomAuth.Core.Constants;
using CusstomAuth.Core.Endpoints;
using CusstomAuth.Core.Extensions;
using CusstomAuth.Core.Managers;
using CusstomAuth.Core.Options;
using CusstomAuth.Core.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace CusstomAuth.Core.Features.Devices;

public class GetDevicesEndpointHandler : IEndpointHandler
{
    public GetDevicesEndpointHandler()
    {

    }

    public GetDevicesEndpointHandler(string action, EndpointOptions endpoint)
    {
        Action = action;
        Endpoint = endpoint;
    }

    public string Action { get; } = HttpActions.GetDevicesAction;

    public EndpointOptions Endpoint { get; }

    public IEndpointHandler CreateFromOptions(Dictionary<string, EndpointOptions> options)
    {
        var currentOption = options[Action];
        return new GetDevicesEndpointHandler(Action, currentOption);
    }

    public async Task HandleAsync(HttpContext context)
    {
        string[] deviceIds = context.Request.Query["deviceIds"]!;

        var deviceService = context.RequestServices.GetRequiredService<IDeviceManager>();

        var result = await deviceService.GetUserDevicesAsync(deviceIds);

        await context.Response.WriteAsJsonAsync(ApiResponse.Success(200, result));
    }
}
