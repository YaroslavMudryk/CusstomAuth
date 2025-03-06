using CusstomAuth.Core.Constants;
using CusstomAuth.Core.Features.Devices.Services;
using CusstomAuth.Core.Handlers;
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
        var deviceService = context.RequestServices.GetRequiredService<IDeviceService>();

        var result = await deviceService.GetUserDevicesAsync();

        await context.Response.WriteAsJsonAsync(ApiResponse.Success(200, result));
    }
}
