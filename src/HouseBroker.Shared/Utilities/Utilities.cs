using System.Text.Json;

namespace HouseBroker.Shared.Utilities;

public class Utilities
{
    public static T DeepClone<T>(T obj)
    {
        if (obj == null)
            return default;

        var json = JsonSerializer.Serialize(obj);
        return JsonSerializer.Deserialize<T>(json);
    }
}