using CusstomAuth.Core.Features.Confirm.Services;
using CusstomAuth.Core.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace CusstomAuth.Core.Features.Confirm;

public static class ConfirmDependencies
{
    public static void Register(IServiceCollection services)
    {
        services.AddScoped<IConfirmUserService, ConfirmUserService>();
        services.AddScoped<IEndpointHandler, ConfirmEndpointHandler>();
        services.AddScoped<IEndpointHandler, SendConfirmEndpointHandler>();
    }
}
