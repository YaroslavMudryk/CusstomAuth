using CusstomAuth.Core.Constants;
using CusstomAuth.Core.Extensions;
using CusstomAuth.Core.Features.SignUp.Dtos;
using CusstomAuth.Core.Features.SignUp.Services;
using CusstomAuth.Core.Handlers;
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
        var registerBody = await JsonSerializer.DeserializeAsync<SignUpDto>(context.Request.Body, Settings.Api);

        var validator = context.RequestServices.GetRequiredService<IValidator<SignUpDto>>();

        await validator.ValidateAndThrowAsync(registerBody);

        var signUpService = context.RequestServices.GetRequiredService<ISignUpService>();

        var createdUserId = await signUpService.SignUpAsync(registerBody);

        await context.Response.WriteAsJsonAsync(ApiResponse.Success(200, createdUserId));
    }
}
