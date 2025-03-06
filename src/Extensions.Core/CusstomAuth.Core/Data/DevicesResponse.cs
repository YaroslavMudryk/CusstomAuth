namespace CusstomAuth.Core.Data;

public class DevicesResponse
{
    public IReadOnlyList<DeviceInfo> Devices { get; set; } = default!;
}
