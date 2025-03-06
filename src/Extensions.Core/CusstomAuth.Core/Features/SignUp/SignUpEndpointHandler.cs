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

namespace CusstomAuth.Core.Features.SignUp;

public class SignUpEndpointHandler : IEndpointHandler
{
    public SignUpEndpointHandler()
    {

    }

    public SignUpEndpointHandler(string action, EndpointOptions endpoint)
    {
        Action = action;
        Endpoint = endpoint;
    }
    public string Action { get; } = HttpActions.SignUpAction;

    public EndpointOptions Endpoint { get; }

    public IEndpointHandler CreateFromOptions(Dictionary<string, EndpointOptions> options)
    {
        var currentOption = options[Action];
        return new SignUpEndpointHandler(Action, currentOption);
    }

    public async Task HandleAsync(HttpContext context)
    {
        var registerBody = await JsonSerializer.DeserializeAsync<SignUpRequest>(context.Request.Body, Settings.Api);

        var validator = context.RequestServices.GetRequiredService<IValidator<SignUpRequest>>();

        await ValidateAndThrowAsync(validator, registerBody!);

        var signUpManager = context.RequestServices.GetRequiredService<ISignUpManager>();

        var createdUserId = await signUpManager.SignUpAsync(registerBody!);

        await context.Response.WriteAsJsonAsync(ApiResponse.Success(200, createdUserId));
    }

    private static async Task ValidateAndThrowAsync(IValidator<SignUpRequest> validator, SignUpRequest signInRequest)
    {
        var validateResult = await validator.ValidateAsync(signInRequest);

        if (!validateResult.IsValid)
        {
            throw new FailedValidationException(validateResult.MapToFailedValidation());
        }
    }
}
