using CusstomAuth.Core.Constants;
using CusstomAuth.Core.Endpoints;
using CusstomAuth.Core.Extensions;
using CusstomAuth.Core.Managers;
using CusstomAuth.Core.Options;
using CusstomAuth.Core.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace CusstomAuth.Core.Features.Confirm;

public class ConfirmEndpointHandler : IEndpointHandler
{
    public ConfirmEndpointHandler()
    {

    }

    public ConfirmEndpointHandler(string action, EndpointOptions endpoint)
    {
        Action = action;
        Endpoint = endpoint;
    }

    public string Action { get; } = HttpActions.ConfirmAction;

    public EndpointOptions Endpoint { get; }

    public IEndpointHandler CreateFromOptions(Dictionary<string, EndpointOptions> options)
    {
        var currentOption = options[Action];
        return new ConfirmEndpointHandler(Action, currentOption);
    }

    public async Task HandleAsync(HttpContext context)
    {
        string code = context.Request.Query["code"]!;

        int userId = Convert.ToInt32(context.Request.Query["userId"]);

        var confirmService = context.RequestServices.GetRequiredService<IConfirmManager>();

        var result = await confirmService.ConfirmUserAsync(code, userId);

        await context.Response.WriteAsJsonAsync(ApiResponse.Success(200, result));
    }
}
