using Microsoft.Extensions.DependencyInjection;

namespace CusstomAuth.Core.Options;

public class SessionManagerOptions
{
    public Type Implementation { get; set; } = null!;
    public ServiceLifetime Lifetime { get; set; } = ServiceLifetime.Scoped;
}
