namespace CusstomAuth.Core.Options;

public class ServicesOptions
{
    public Type EmailImplementation { get; set; } = null;
    public Type LocationImplementation { get; set; } = null;
    public Type NotificationImplementation { get; set; } = null;
    public Type SmsImplementation { get; set; } = null;
}
