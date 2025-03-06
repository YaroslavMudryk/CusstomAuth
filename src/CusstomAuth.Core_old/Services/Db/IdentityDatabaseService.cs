using CusstomAuth.Core.Constants;
using CusstomAuth.Core.Db;
using CusstomAuth.Core.Db.Entities;
using CusstomAuth.Core.Helpers;
using Microsoft.EntityFrameworkCore;

namespace CusstomAuth.Core.Services.Db;

public interface IDatabaseService
{
    Task<bool> CreateDbAsync();
    Task<int> SeedSystemAsync();
}

public class IdentityDatabaseService(
    AuthDbContext db,
    TimeProvider dateTimeProvider) : IDatabaseService
{
    public async Task<int> SeedSystemAsync()
    {
        int counter = 0;
        if (!await db.Roles.AnyAsync())
        {
            await db.Roles.AddRangeAsync(GetDefaultsRoles());
            counter++;
        }
        if (!await db.Apps.AnyAsync())
        {
            await db.Apps.AddRangeAsync(GetDefaultsApp());
            counter++;
        }

        if (counter > 0)
            await db.SaveChangesAsync();

        return counter;
    }

    private static IEnumerable<AuthRole> GetDefaultsRoles()
    {
        yield return new AuthRole
        {
            Name = DefaultsRoles.Administrator,
            IsDefault = false,
            NameNormalized = DefaultsRoles.Administrator.ToUpper(),
        };
        yield return new AuthRole
        {
            Name = DefaultsRoles.User,
            IsDefault = true,
            NameNormalized = DefaultsRoles.User.ToUpper(),
        };
    }

    private IEnumerable<AuthApp> GetDefaultsApp()
    {
        var utcNow = dateTimeProvider.GetUtcNow().UtcDateTime;

        yield return new AuthApp
        {
            Name = "Application",
            Description = "Application for development",
            IsActive = true,
            ShortName = "App",
            ActiveFrom = utcNow,
            ActiveTo = utcNow.AddYears(5),
            ClientId = Generator.CreateAppId(),
            ClientSecret = Generator.CreateAppSecret(),
        };
    }

    public async Task<bool> CreateDbAsync()
    {
        return await db.Database.EnsureCreatedAsync();
    }
}
