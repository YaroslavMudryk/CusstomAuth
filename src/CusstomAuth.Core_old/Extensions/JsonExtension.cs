using System.Text.Json;

namespace CusstomAuth.Core.Extensions;

public static class JsonExtension
{
    public static string ToJson(this object data)
    {
        return JsonSerializer.Serialize(data, Settings.EntityFramework);
    }

    public static T FromJson<T>(this string json)
    {
        return JsonSerializer.Deserialize<T>(json, Settings.EntityFramework);
    }
}
