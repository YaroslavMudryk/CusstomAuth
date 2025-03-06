using CusstomAuth.Core.Features.SignUp.Dtos;
using CusstomAuth.Core.Features.SignUp.Dtos.Validators;
using CusstomAuth.Core.Features.SignUp.Services;
using CusstomAuth.Core.Handlers;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CusstomAuth.Core.Features.SignUp;

public static class SignUpDependencies
{
    public static void Register(IServiceCollection services)
    {
        services.AddScoped<IEndpointHandler, SignUpEndpointHandler>();
        services.AddScoped<ISignUpService, SignUpService>();

        services.AddScoped<IValidator<SignUpDto>, SignUpDtoValidator>();
    }
}
