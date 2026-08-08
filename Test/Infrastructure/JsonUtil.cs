using System.Text.Json;

namespace Test.Infrastructure;

public static class JsonUtil
{
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    public static string ToJson<T>(T value) => JsonSerializer.Serialize(value, Options);
    public static void WriteJsonFile<T>(string path, T value) => File.WriteAllText(path, ToJson(value));
}