using Microsoft.Extensions.DependencyInjection;

namespace CusstomAuth;

public class IdentityBuilder
{
    public IdentityBuilder(IServiceCollection services)
    {
        Services = services;
    }

    public IServiceCollection Services { get; }

    private IdentityBuilder AddScoped(Type serviceType, Type concreteType)
    {
        Services.AddScoped(serviceType, concreteType);
        return this;
    }
}
