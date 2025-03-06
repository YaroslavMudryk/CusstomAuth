using CusstomAuth.Core.Constants;
using CusstomAuth.Core.Features.Confirm.Services;
using CusstomAuth.Core.Handlers;
using CusstomAuth.Core.Options;
using CusstomAuth.Core.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace CusstomAuth.Core.Features.Confirm;

public class SendConfirmEndpointHandler : IEndpointHandler
{
    public SendConfirmEndpointHandler()
    {

    }

    public SendConfirmEndpointHandler(string action, EndpointOptions endpoint)
    {
        Action = action;
        Endpoint = endpoint;
    }

    public string Action { get; } = HttpActions.SendConfirmAction;

    public EndpointOptions Endpoint { get; }

    public IEndpointHandler CreateFromOptions(Dictionary<string, EndpointOptions> options)
    {
        var currentOption = options[Action];
        return new SendConfirmEndpointHandler(Action, currentOption);
    }

    public async Task HandleAsync(HttpContext context)
    {
        int userId = Convert.ToInt32(context.Request.Query["userId"]);

        var confirmService = context.RequestServices.GetRequiredService<IConfirmUserService>();

        var result = await confirmService.SendConfirmAsync(userId);

        await context.Response.WriteAsJsonAsync(ApiResponse.Success(200, result));
    }
}
