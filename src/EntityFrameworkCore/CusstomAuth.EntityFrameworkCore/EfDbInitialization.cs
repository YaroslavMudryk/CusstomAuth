using CusstomAuth.Core.Constants;
using CusstomAuth.Core.Data;
using CusstomAuth.Core.Helpers;
using CusstomAuth.Core.Initialization;
using CusstomAuth.Core.Managers;
using CusstomAuth.Core.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CusstomAuth.EntityFrameworkCore;

public class EfDbInitialization(
    IdentityDbContext dbContext,
    TimeProvider timeProvider,
    ISignUpManager signUpManager,
    ISignInManager signInManager,
    IOptions<IdentityOptions> options) : IDbInitialization
{
    public async Task<InitializeResponse> InitializeAsync()
    {
        int counter = 0;
        if (!await dbContext.Apps.AnyAsync())
        {
            await dbContext.Apps.AddRangeAsync(GetDefaultsApp());
            counter++;
        }
        if (!await dbContext.Roles.AnyAsync())
        {
            await dbContext.Roles.AddRangeAsync(GetDefaultsRoles());
            counter++;
        }
        if (counter > 0)
            await dbContext.SaveChangesAsync();

        return await GenerateResponseAsync(counter);
    }

    private async Task<InitializeResponse> GenerateResponseAsync(int counter)
    {
        if (counter == 0)
            return new InitializeResponse();

        var apps = dbContext.Apps.Local.Select(app => new AppInitDto
        {
            Id = app.Id,
            Name = app.Name,
            ShortName = app.ShortName,
            ClientId = app.ClientId,
            ClientSecret = app.ClientSecret
        }).ToList();

        var signUpRequest = new SignUpRequest
        {
            FirstName = "Admin",
            LastName = "Admin",
            Login = "mail@email.com",
            Password = Generator.GetPassword(options.Value.Password)
        };
        var sigUpResult = await signUpManager.SignUpAsync(signUpRequest, true);

        var neededApp = apps.FirstOrDefault(app => app.ShortName == "Web");
        var singInRequest = new SignInRequest
        {
            App = new AppSignInDto
            {
                Id = neededApp!.ClientId,
                Secret = neededApp!.ClientSecret,
                Version = "0.0.1"
            },
            Device = new DeviceInfo
            {
                DeviceIdentifier = Guid.CreateVersion7().ToString(),
                Browser = "browser"
            },
            Lang = "en",
            Login = signUpRequest.Login,
            Password = signUpRequest.Password
        };
        var sigInResult = await signInManager.SignInAsync(singInRequest);

        return new InitializeResponse
        {
            Apps = apps,
            User = new UserInitDto
            {
                Id = sigUpResult.UserId,
                Name = $"{signUpRequest.FirstName} {signUpRequest.LastName}",
                Login = signUpRequest.Login,
                Password = signUpRequest.Password,
                AccessToken = sigInResult.Token,
                RefreshToken = sigInResult.RefreshToken,
            }
        };
    }

    private static IEnumerable<IdentityRole> GetDefaultsRoles()
    {
        yield return new IdentityRole
        {
            Name = DefaultsRoles.Administrator,
            IsDefault = false,
            NameNormalized = DefaultsRoles.Administrator.ToUpper(),
        };
        yield return new IdentityRole
        {
            Name = DefaultsRoles.User,
            IsDefault = true,
            NameNormalized = DefaultsRoles.User.ToUpper(),
        };
    }

    private IEnumerable<IdentityApp> GetDefaultsApp()
    {
        var utcNow = timeProvider.GetUtcNow().UtcDateTime;

        yield return new IdentityApp
        {
            Name = "Web application",
            Description = "Web application from seed",
            IsActive = true,
            ShortName = "Web",
            ActiveFrom = utcNow,
            ActiveTo = utcNow.AddYears(5),
            ClientId = Generator.CreateAppId(),
            ClientSecret = Generator.CreateAppSecret(),
        };

        yield return new IdentityApp
        {
            Name = "Android application",
            Description = "Android application from seed",
            IsActive = true,
            ShortName = "Android",
            ActiveFrom = utcNow,
            ActiveTo = utcNow.AddYears(5),
            ClientId = Generator.CreateAppId(),
            ClientSecret = Generator.CreateAppSecret(),
        };

        yield return new IdentityApp
        {
            Name = "iOS application",
            Description = "iOS application from seed",
            IsActive = true,
            ShortName = "iOS",
            ActiveFrom = utcNow,
            ActiveTo = utcNow.AddYears(5),
            ClientId = Generator.CreateAppId(),
            ClientSecret = Generator.CreateAppSecret(),
        };

        yield return new IdentityApp
        {
            Name = "IoT application",
            Description = "IoT application from seed",
            IsActive = true,
            ShortName = "IoT",
            ActiveFrom = utcNow,
            ActiveTo = utcNow.AddYears(5),
            ClientId = Generator.CreateAppId(),
            ClientSecret = Generator.CreateAppSecret(),
        };
    }
}
