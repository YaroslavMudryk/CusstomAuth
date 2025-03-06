using CusstomAuth.Core.Services.Db;
using Microsoft.Extensions.DependencyInjection;

namespace CusstomAuth.Core.Seeding;

public static class SeedDependencies
{
    public static void Register(IServiceCollection services)
    {
        services.AddScoped<IDatabaseService, IdentityDatabaseService>();
    }
}
