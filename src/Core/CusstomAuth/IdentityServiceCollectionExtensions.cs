using CusstomAuth.Core.Data;
using CusstomAuth.Core.Endpoints;
using CusstomAuth.Core.Features.Confirm;
using CusstomAuth.Core.Features.Devices;
using CusstomAuth.Core.Features.Init;
using CusstomAuth.Core.Features.Mfa;
using CusstomAuth.Core.Features.RefreshToken;
using CusstomAuth.Core.Features.Sessions;
using CusstomAuth.Core.Features.SignIn;
using CusstomAuth.Core.Features.SignOut;
using CusstomAuth.Core.Features.SignUp;
using CusstomAuth.Core.Initialization;
using CusstomAuth.Core.Managers;
using CusstomAuth.Core.Notifyer;
using CusstomAuth.Core.Options;
using CusstomAuth.Core.Passwords;
using CusstomAuth.Core.Services;
using CusstomAuth.Core.SessionsManagement;
using Extensions.DeviceDetector;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CusstomAuth;

public static class IdentityServiceCollectionExtensions
{
    public static IdentityBuilder AddIdentityCore2(this IServiceCollection services)
        => services.AddIdentityCore2(o => { });

    public static IdentityBuilder AddIdentityCore2(this IServiceCollection services, Action<IdentityOptions> identityOptions)
    {
        IdentityOptions options = new();
        if (identityOptions != null)
        {
            identityOptions.Invoke(options);
            services.Configure(identityOptions);
        }

        services.AddHandlers();
        services.AddServices();
        services.AddDefaultNotifyer();
        services.AddDefaultManagers();
        services.AddDefaultPasswordHasher();
        services.AddContext();
        services.AddValidators();
        services.AddScoped<IEndpointService, EndpointService>();
        services.AddSingleton((sp) => TimeProvider.System);
        services.AddDeviceDetector();
        services.AddHttpContextAccessor();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                        .AddJwtBearer(jwt =>
                        {
                            jwt.RequireHttpsMetadata = false;
                            jwt.SaveToken = true;
                            jwt.TokenValidationParameters = new TokenValidationParameters
                            {
                                ValidateIssuer = true,
                                ValidIssuer = options.Token.Issuer,
                                ValidateAudience = true,
                                ValidateLifetime = true,
                                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(options.Token.SecretKey)),
                                ValidateIssuerSigningKey = true
                            };
                        });

        return new IdentityBuilder(services);
    }

    private static void AddHandlers(this IServiceCollection services)
    {
        services.AddScoped<IEndpointHandler, SeedEndpointHandler>();
        services.AddScoped<IEndpointHandler, ConfirmEndpointHandler>();
        services.AddScoped<IEndpointHandler, SendConfirmEndpointHandler>();
        services.AddScoped<IEndpointHandler, GetDevicesEndpointHandler>();
        services.AddScoped<IEndpointHandler, TurnOnMfaEndpointHandler>();
        services.AddScoped<IEndpointHandler, TurnOffMfaEndpointHandler>();
        services.AddScoped<IEndpointHandler, RefreshTokenEndpointHandler>();
        services.AddScoped<IEndpointHandler, SessionsEndpointHandler>();
        services.AddScoped<IEndpointHandler, CloseSessionsEndpointHandler>();
        services.AddScoped<IEndpointHandler, SignUpEndpointHandler>();
        services.AddScoped<IEndpointHandler, SignInEndpointHandler>();
        services.AddScoped<IEndpointHandler, SignOutEndpointHandler>();
    }

    private static void AddDefaultManagers(this IServiceCollection services)
    {
        services.AddScoped<IConfirmManager, ConfirmManager>();
        services.AddScoped<IDeviceManager, DeviceManager>();
        services.AddScoped<IMfaManager, MfaManager>();
        services.AddScoped<IRefreshTokenManager, RefreshTokenManager>();
        services.AddScoped<ISessionManager, SessionManager>();
        services.AddScoped<IDeviceManager, DeviceManager>();
        services.AddScoped<ISignUpManager, SignUpManager>();
        services.AddScoped<ISignInManager, SignInManager>();
        services.AddScoped<ISignOutManager, SignOutManager>();
    }

    private static void AddDefaultPasswordHasher(this IServiceCollection services)
    {
        services.AddScoped<IPasswordHasher, DefaultPasswordHasher>();
    }

    public static void AddDefaultNotifyer(this IServiceCollection services)
    {
        services.AddScoped<INotifyer, FakeNotifyer>();
    }

    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IDeviceService, DeviceService>();
        services.AddScoped<ILocationService, FakeLocationService>();
        services.AddScoped<ITokenService, TokenService>();
    }

    public static void AddContext(this IServiceCollection services)
    {
        services.AddScoped<ICurrentContext, HttpCurrentContext>();
    }

    public static void AddValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<SignInRequest>, SignInRequestValidator>();
        services.AddScoped<IValidator<SignUpRequest>, SignUpRequestValidator>();
    }
}
