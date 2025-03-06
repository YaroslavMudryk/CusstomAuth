
using CusstomAuth;
using CusstomAuth.EntityFrameworkCore;

namespace IdentityExample;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddAuthorization();
        builder.Services
            .AddIdentityCore2()
            .AddEntityFrameworkStores<IdentityDbContext>(options =>
            {
                options.DatabaseProvider = CusstomAuth.Core.Options.DatabaseProviders.InMemory;
            })
            .AddEFSessionManagment();

        builder.Services.AddOpenApi();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapIdentityApi();

        app.Run();
    }
}
