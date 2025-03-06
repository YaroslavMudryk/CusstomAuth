using CusstomAuth.Core.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace CusstomAuth.Core.Features.Init;

public static class InitDependecies
{
    public static void Register(IServiceCollection services)
    {
        services.AddScoped<IEndpointHandler, InitEndpointHandler>();
    }
}
