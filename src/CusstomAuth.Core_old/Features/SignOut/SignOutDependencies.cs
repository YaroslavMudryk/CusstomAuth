using CusstomAuth.Core.Features.SignOut.Services;
using CusstomAuth.Core.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace CusstomAuth.Core.Features.SignOut;

public static class SignOutDependencies
{
    public static void Register(IServiceCollection services)
    {
        services.AddScoped<ISignOutService, SignOutService>();
        services.AddScoped<IEndpointHandler, SignOutEndpointHandler>();
    }
}
