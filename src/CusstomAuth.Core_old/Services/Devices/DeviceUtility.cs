using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace CusstomAuth.Core.Services.Devices;

public class DeviceUtility
{
    private static JsonSerializerOptions _options = new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
    public static string GenerateHash(DeviceDetails device)
    {
        var json = JsonSerializer.Serialize(device, _options);
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
    }

    public static ValidationStatus ValidateHash(DeviceDetails device, string oldHash)
    {
        var oldDeviceDetails = GetDeviceDetails(oldHash);

        if (device.GetBrowserHash() != oldDeviceDetails.GetBrowserHash())
            return ValidationStatus.Failed;

        if (device.GetDeviceHash() != oldDeviceDetails.GetDeviceHash())
            return ValidationStatus.Failed;

        if (device.GetOsHash() != oldDeviceDetails.GetOsHash())
        {
            if (device.Os != oldDeviceDetails.Os || device.OsUI != oldDeviceDetails.OsUI || device.OsPlatform != oldDeviceDetails.OsPlatform || device.OsShortName != oldDeviceDetails.OsShortName)
                return ValidationStatus.Failed;

            if (Convert.ToDouble(device.OsVersion) > Convert.ToDouble(oldDeviceDetails.OsVersion))
                return ValidationStatus.OsUpdated;

            if (Convert.ToDouble(device.OsVersion) < Convert.ToDouble(oldDeviceDetails.OsVersion))
                return ValidationStatus.OsDowngraded;
        }

        return ValidationStatus.Success;
    }

    private static DeviceDetails GetDeviceDetails(string hash)
    {
        return JsonSerializer.Deserialize<DeviceDetails>(Encoding.UTF8.GetString(Convert.FromBase64String(hash)), _options);
    }
}
