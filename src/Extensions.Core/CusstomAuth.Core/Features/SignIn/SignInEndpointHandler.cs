using CusstomAuth.Core.Constants;
using CusstomAuth.Core.Data;
using CusstomAuth.Core.Endpoints;
using CusstomAuth.Core.ErrorHandling.Exceptions;
using CusstomAuth.Core.ErrorHandling.Extensions;
using CusstomAuth.Core.Extensions;
using CusstomAuth.Core.Managers;
using CusstomAuth.Core.Options;
using CusstomAuth.Core.Responses;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;

namespace CusstomAuth.Core.Features.SignIn;

public class SignInEndpointHandler : IEndpointHandler
{
    public SignInEndpointHandler()
    {

    }

    public SignInEndpointHandler(string action, EndpointOptions endpoint)
    {
        Action = action;
        Endpoint = endpoint;
    }

    public string Action { get; } = HttpActions.SignInAction;

    public EndpointOptions Endpoint { get; }

    public IEndpointHandler CreateFromOptions(Dictionary<string, EndpointOptions> options)
    {
        var currentOption = options[Action];
        return new SignInEndpointHandler(Action, currentOption);
    }

    public async Task HandleAsync(HttpContext context)
    {
        var signInBody = await JsonSerializer.DeserializeAsync<SignInRequest>(context.Request.Body, Settings.Api);

        var validator = context.RequestServices.GetRequiredService<IValidator<SignInRequest>>();

        await ValidateAndThrowAsync(validator, signInBody!);

        var signInManager = context.RequestServices.GetRequiredService<ISignInManager>();

        var token = await signInManager.SignInAsync(signInBody!);

        await context.Response.WriteAsJsonAsync(ApiResponse.Success(200, token));
    }

    private static async Task ValidateAndThrowAsync(IValidator<SignInRequest> validator, SignInRequest signInRequest)
    {
        var validateResult = await validator.ValidateAsync(signInRequest);

        if (!validateResult.IsValid)
        {
            throw new FailedValidationException(validateResult.MapToFailedValidation());
        }
    }
}
