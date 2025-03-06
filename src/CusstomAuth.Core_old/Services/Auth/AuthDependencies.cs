using Microsoft.Extensions.DependencyInjection;

namespace CusstomAuth.Core.Services.Auth;

public static class AuthDependencies
{
    public static void Register(IServiceCollection services)
    {
        services.AddScoped<ICurrentContext, HttpCurrentContext>();
        services.AddScoped<IEndpointService, EndpointService>();
    }
}
