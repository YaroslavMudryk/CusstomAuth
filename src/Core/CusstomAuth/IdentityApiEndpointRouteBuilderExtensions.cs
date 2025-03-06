using CusstomAuth.Core.ErrorHandling;
using CusstomAuth.Core.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace CusstomAuth;

public static class IdentityApiEndpointRouteBuilderExtensions
{
    public static void MapIdentityApi(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<IdentityErrorHandlerMiddleware>();
        builder.UseMiddleware<IdentityMiddleware>();
    }
}
