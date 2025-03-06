using CusstomAuth.Core.Constants;
using CusstomAuth.Core.Extensions;
using CusstomAuth.Core.Features.SignIn.Dtos;
using CusstomAuth.Core.Features.SignIn.Services;
using CusstomAuth.Core.Handlers;
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
        var signInBody = await JsonSerializer.DeserializeAsync<SignInDto>(context.Request.Body, Settings.Api);

        var validator = context.RequestServices.GetRequiredService<IValidator<SignInDto>>();

        await validator.ValidateAndThrowAsync(signInBody);

        var signInService = context.RequestServices.GetRequiredService<ISignInService>();

        var token = await signInService.SignInByPasswordAsync(signInBody);

        await context.Response.WriteAsJsonAsync(ApiResponse.Success(200, token));
    }
}
