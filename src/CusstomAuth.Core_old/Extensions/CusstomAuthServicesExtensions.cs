using CusstomAuth.Core.Db;
using CusstomAuth.Core.Db.Audits;
using CusstomAuth.Core.ErrorHandling;
using CusstomAuth.Core.Features.Confirm;
using CusstomAuth.Core.Features.Devices;
using CusstomAuth.Core.Features.Init;
using CusstomAuth.Core.Features.Mfa;
using CusstomAuth.Core.Features.RefreshToken;
using CusstomAuth.Core.Features.Sessions;
using CusstomAuth.Core.Features.SignIn;
using CusstomAuth.Core.Features.SignOut;
using CusstomAuth.Core.Features.SignUp;
using CusstomAuth.Core.Managers;
using CusstomAuth.Core.Middlewares;
using CusstomAuth.Core.Options;
using CusstomAuth.Core.Seeding;
using CusstomAuth.Core.Services.Auth;
using CusstomAuth.Core.Services.Auth.SessionsManager;
using CusstomAuth.Core.Services.Email;
using CusstomAuth.Core.Services.Location;
using CusstomAuth.Core.Services.Notifications;
using CusstomAuth.Core.Services.Sms;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using System.Text;

namespace CusstomAuth.Core.Extensions;

public static class CusstomAuthServicesExtensions
{
    public static IServiceCollection AddCusstomAuth(this IServiceCollection services)
    {
        return services.AddCusstomAuth(new CusstomAuthOptions());
    }

    public static IServiceCollection AddCusstomAuth(this IServiceCollection services, Action<CusstomAuthOptions> options)
    {
        var customAuthOptions = new CusstomAuthOptions();
        if (options != null)
        {
            services.Configure(options);
            options(customAuthOptions);
        }
        return services.AddCusstomAuth(customAuthOptions);
    }

    private static IServiceCollection AddCusstomAuth(this IServiceCollection services, CusstomAuthOptions authOptions)
    {
        ManagersDependencies.Register(services);
        SignUpDependencies.Register(services);
        AuthDependencies.Register(services);
        SignInDependencies.Register(services);
        SessionsDependencies.Register(services);
        ConfirmDependencies.Register(services);
        RefreshTokenDependencies.Register(services);
        SignOutDependencies.Register(services);
        MfaDependencies.Register(services);
        DevicesDependencies.Register(services);
        SeedDependencies.Register(services);
        InitDependecies.Register(services);

        services.AddScoped<ICurrentContext, HttpCurrentContext>();
        services.AddScoped<AuditRepo>();

        ConfigureImplServices(services, authOptions);

        services.AddSingleton(TimeProvider.System);
        services.AddHttpContextAccessor();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                        .AddJwtBearer(jwt =>
                        {
                            jwt.RequireHttpsMetadata = false;
                            jwt.SaveToken = true;
                            jwt.TokenValidationParameters = new TokenValidationParameters
                            {
                                ValidateIssuer = true,
                                ValidIssuer = authOptions.Token.Issuer,
                                ValidateAudience = false,
                                ValidateLifetime = true,
                                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(authOptions.Token.SecretKey)),
                                ValidateIssuerSigningKey = true
                            };
                        });

        var db = authOptions.DbConnection;

        services.AddDbContext<AuthDbContext>(options =>
        {
            if (db.DatabaseProvider == DatabaseProviders.Sqlite)
            {
                options.UseSqlite(db.ConnectionString);
            }
            if (db.DatabaseProvider == DatabaseProviders.SqlServer)
            {
                options.UseSqlServer(db.ConnectionString);
            }
            if (db.DatabaseProvider == DatabaseProviders.Postgres)
            {
                NpgsqlConnection.GlobalTypeMapper.EnableDynamicJson();
                AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
                options.UseNpgsql(db.ConnectionString);
            }
        });

        return services;
    }

    private static void ConfigureImplServices(IServiceCollection services, CusstomAuthOptions authOptions)
    {
        var sessionManager = authOptions.Token.SessionManager;
        if (sessionManager.Implementation != null && typeof(ISessionManager).IsAssignableFrom(sessionManager.Implementation))
        {
            if (sessionManager.Lifetime == ServiceLifetime.Transient)
                services.AddTransient(typeof(ISessionManager), sessionManager.Implementation);
            if (sessionManager.Lifetime == ServiceLifetime.Scoped)
                services.AddScoped(typeof(ISessionManager), sessionManager.Implementation);
            if (sessionManager.Lifetime == ServiceLifetime.Singleton)
                services.AddSingleton(typeof(ISessionManager), sessionManager.Implementation);
        }
        else
        {
            services.AddSingleton<ISessionManager, InMemorySessionManager>();
        }

        var emailService = authOptions.Services.EmailImplementation;
        if (emailService != null && typeof(IEmailService).IsAssignableFrom(emailService))
            services.AddScoped(typeof(IEmailService), emailService);
        else
            services.AddScoped<IEmailService, FakeEmailService>();

        var locationService = authOptions.Services.LocationImplementation;
        if (locationService != null && typeof(ILocationService).IsAssignableFrom(locationService))
            services.AddScoped(typeof(ILocationService), locationService);
        else
            services.AddScoped<ILocationService, FakeLocationService>();

        var notificationService = authOptions.Services.NotificationImplementation;
        if (notificationService != null && typeof(INotificationService).IsAssignableFrom(notificationService))
            services.AddScoped(typeof(INotificationService), notificationService);
        else
            services.AddScoped<INotificationService, FakeNotificationService>();

        var smsService = authOptions.Services.SmsImplementation;
        if (smsService != null && typeof(ISmsService).IsAssignableFrom(smsService))
            services.AddScoped(typeof(ISmsService), smsService);
        else
            services.AddScoped<ISmsService, FakeSmsService>();
    }

    public static void UseCusstomAuth(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<GlobalErrorHandlerMiddleware>();
        builder.UseMiddleware<CusstomAuthMiddleware>();
    }
}
