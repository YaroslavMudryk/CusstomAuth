using CusstomAuth.Core.Constants;
using CusstomAuth.Core.Db;
using CusstomAuth.Core.Extensions;
using CusstomAuth.Core.Handlers;
using CusstomAuth.Core.Options;
using CusstomAuth.Core.Responses;
using CusstomAuth.Core.Services.Db;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CusstomAuth.Core.Features.Init;

public class InitEndpointHandler : IEndpointHandler
{

    public InitEndpointHandler()
    {

    }

    public InitEndpointHandler(string action, EndpointOptions endpoint)
    {
        Action = action;
        Endpoint = endpoint;
    }

    public string Action { get; } = HttpActions.InitAction;

    public EndpointOptions Endpoint { get; }

    public IEndpointHandler CreateFromOptions(Dictionary<string, EndpointOptions> options)
    {
        var currentOption = options[Action];
        return new InitEndpointHandler(Action, currentOption);
    }

    public async Task HandleAsync(HttpContext context)
    {
        var dbServices = context.RequestServices.GetRequiredService<IEnumerable<IDatabaseService>>();

        var firstService = dbServices.First();

        await firstService.CreateDbAsync();
        await firstService.SeedSystemAsync();

        foreach (var dbService in dbServices.Skip(1))
        {
            await dbService.SeedSystemAsync();
        }

        var authDbContext = context.RequestServices.GetRequiredService<AuthDbContext>();
        var app = await authDbContext.Apps.AsNoTracking().FirstOrDefaultAsync();

        await context.Response.WriteAsJsonAsync(ApiResponse.Success(200,
            new
            {
                AppId = app.Id,
                ClientId = app.ClientId,
                ClientSecret = app.ClientSecret,
            }), Settings.Api);
    }
}
