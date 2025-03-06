using CusstomAuth.Core.Features.Mfa.Services;
using CusstomAuth.Core.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace CusstomAuth.Core.Features.Mfa;

public static class MfaDependencies
{
    public static void Register(IServiceCollection services)
    {
        services.AddScoped<IMfaService, MfaService>();
        services.AddScoped<IEndpointHandler, TurnOnMfaEndpointHandler>();
        services.AddScoped<IEndpointHandler, TurnOffMfaEndpointHandler>();
    }
}
