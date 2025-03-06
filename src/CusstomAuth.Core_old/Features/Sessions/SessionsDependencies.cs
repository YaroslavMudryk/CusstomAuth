using CusstomAuth.Core.Features.Sessions.Services;
using CusstomAuth.Core.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace CusstomAuth.Core.Features.Sessions;

public static class SessionsDependencies
{
    public static void Register(IServiceCollection services)
    {
        services.AddScoped<ISessionService, SessionService>();
        services.AddScoped<IEndpointHandler, SessionsEndpointHandler>();
        services.AddScoped<IEndpointHandler, CloseSessionsEndpointHandler>();
    }
}
