using Microsoft.Extensions.DependencyInjection;

namespace CusstomAuth.Core.Managers;

public static class ManagersDependencies
{
    public static void Register(IServiceCollection services)
    {
        services.AddScoped<ISignInManager, SignInManager>();
        services.AddScoped<ISignUpManager, SignUpManager>();
        services.AddScoped<IUserManager, UserManager>();
        services.AddScoped<IRoleManager, RoleManager>();
    }
}
