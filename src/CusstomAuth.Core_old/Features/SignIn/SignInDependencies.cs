using CusstomAuth.Core.Features.SignIn.Dtos;
using CusstomAuth.Core.Features.SignIn.Dtos.Validators;
using CusstomAuth.Core.Features.SignIn.Services;
using CusstomAuth.Core.Handlers;
using Extensions.DeviceDetector;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CusstomAuth.Core.Features.SignIn;

public static class SignInDependencies
{
    public static void Register(IServiceCollection services)
    {
        services.AddDeviceDetector();
        services.AddScoped<IEndpointHandler, SignInEndpointHandler>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ISignInService, SignInService>();
        services.AddScoped<IDeviceService, DeviceService>();
        services.AddScoped<IEndpointHandler, SignInMfaEndpointHandler>();

        services.AddScoped<IValidator<SignInDto>, SignInDtoValidator>();
        services.AddScoped<IValidator<SignInMfaDto>, SignInMfaDtoValidator>();
    }
}
