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

public class SignInMfaEndpointHandler : IEndpointHandler
{
    public SignInMfaEndpointHandler()
    {

    }

    public SignInMfaEndpointHandler(string action, EndpointOptions endpoint)
    {
        Action = action;
        Endpoint = endpoint;
    }

    public string Action { get; } = HttpActions.SignInMfaAction;

    public EndpointOptions Endpoint { get; }

    public IEndpointHandler CreateFromOptions(Dictionary<string, EndpointOptions> options)
    {
        var currentOption = options[Action];
        return new SignInMfaEndpointHandler(Action, currentOption);
    }

    public async Task HandleAsync(HttpContext context)
    {
        var loginMfaBody = await JsonSerializer.DeserializeAsync<SignInMfaDto>(context.Request.Body, Settings.Api);

        var validator = context.RequestServices.GetRequiredService<IValidator<SignInMfaDto>>();

        await validator.ValidateAndThrowAsync(loginMfaBody);

        var authService = context.RequestServices.GetRequiredService<ISignInService>();

        var result = await authService.SignInByMfaAsync(loginMfaBody);

        await context.Response.WriteAsJsonAsync(ApiResponse.Success(200, result));
    }
}
