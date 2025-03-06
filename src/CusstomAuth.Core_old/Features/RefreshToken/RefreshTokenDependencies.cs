using CusstomAuth.Core.Features.RefreshToken.Services;
using CusstomAuth.Core.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace CusstomAuth.Core.Features.RefreshToken;

public static class RefreshTokenDependencies
{
    public static void Register(IServiceCollection services)
    {
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        services.AddScoped<IEndpointHandler, RefreshTokenEndpointHandler>();
    }
}
