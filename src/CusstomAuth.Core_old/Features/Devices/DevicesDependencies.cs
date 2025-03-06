using CusstomAuth.Core.Features.Devices.Services;
using CusstomAuth.Core.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace CusstomAuth.Core.Features.Devices;

public class DevicesDependencies
{
    public static void Register(IServiceCollection services)
    {
        services.AddScoped<IEndpointHandler, GetDevicesEndpointHandler>();
        services.AddScoped<IDeviceService, DeviceService>();
    }
}
