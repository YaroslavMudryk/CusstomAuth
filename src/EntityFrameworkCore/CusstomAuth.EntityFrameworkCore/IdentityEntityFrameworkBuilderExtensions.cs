using CusstomAuth.Core.Initialization;
using CusstomAuth.Core.Options;
using CusstomAuth.Core.SessionsManagement;
using CusstomAuth.Core.Stores;
using CusstomAuth.EntityFrameworkCore.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CusstomAuth.EntityFrameworkCore;

public static class IdentityEntityFrameworkBuilderExtensions
{
    public static IdentityBuilder AddEntityFrameworkStores<TContext>(this IdentityBuilder builder, Action<DbConnectionOptions> dbOptionsAction) where TContext : IdentityDbContext
    {
        ConfigureDbContext<TContext>(builder.Services, dbOptionsAction);
        AddStores<TContext>(builder.Services);
        builder.Services.AddScoped<IDbInitialization, EfDbInitialization>();
        return builder;
    }

    public static IdentityBuilder AddEFSessionManagment(this IdentityBuilder builder)
    {
        builder.Services.AddSingleton<ISessionService>((sp) => new EfSessionService(sp));
        return builder;
    }

    private static void ConfigureDbContext<TContext>(IServiceCollection services, Action<DbConnectionOptions> dbOptionsAction) where TContext : IdentityDbContext
    {
        var dbOptions = new DbConnectionOptions();
        dbOptionsAction?.Invoke(dbOptions);

        services.AddDbContext<TContext>(options =>
        {
            var conString = dbOptions.ConnectionString;
            if (dbOptions.DatabaseProvider == DatabaseProviders.Postgres)
                options.UseNpgsql(conString);
            if (dbOptions.DatabaseProvider == DatabaseProviders.Sqlite)
                options.UseSqlite(conString);
            if (dbOptions.DatabaseProvider == DatabaseProviders.SqlServer)
                options.UseSqlServer(conString);
            else
                options.UseInMemoryDatabase(Guid.NewGuid().ToString("N"));
        });
    }

    private static void AddStores<DbContext>(IServiceCollection services) where DbContext : IdentityDbContext
    {
        services.AddScoped<IAppStore, EfAppStore<DbContext>>();
        services.AddScoped<IAppClaimStore, EfAppClaimStore<DbContext>>();
        services.AddScoped<IBlockStore, EfBlockStore<DbContext>>();
        services.AddScoped<IClaimStore, EfClaimStore<DbContext>>();
        services.AddScoped<IConfirmStore, EfConfirmStore<DbContext>>();
        services.AddScoped<IContactStore, EfContactStore<DbContext>>();
        services.AddScoped<IDeviceStore, EfDeviceStore<DbContext>>();
        services.AddScoped<ILoginAttemptStore, EfLoginAttemptStore<DbContext>>();
        services.AddScoped<IMfaStore, EfMfaStore<DbContext>>();
        services.AddScoped<IPasswordStore, EfPasswordStore<DbContext>>();
        services.AddScoped<IQrStore, EfQrStore<DbContext>>();
        services.AddScoped<IRefreshTokenStore, EfRefreshTokenStore<DbContext>>();
        services.AddScoped<IRoleClaimStore, EfRoleClaimStore<DbContext>>();
        services.AddScoped<IRoleStore, EfRoleStore<DbContext>>();
        services.AddScoped<ISessionStore, EfSessionStore<DbContext>>();
        services.AddScoped<IUserRoleStore, EfUserRoleStore<DbContext>>();
        services.AddScoped<IUserStore, EfUserStore<DbContext>>();
        services.AddScoped<IUserTokenStore, EfUserTokenStore<DbContext>>();
    }
}
