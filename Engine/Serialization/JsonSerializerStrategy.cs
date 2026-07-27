using System.Text.Json;
using Core;

namespace Engine.Serialization;

public class JsonSerializerStrategy : IStorageSerializer
{
    private readonly JsonSerializerOptions _options = new() { WriteIndented = true };

    public string SupportedExtension => ".json";

    public byte[] Serialize<T>(T data) where T : class =>
        JsonSerializer.SerializeToUtf8Bytes(data, _options);

    public T? Deserialize<T>(byte[] bytes) where T : class =>
        JsonSerializer.Deserialize<T>(bytes, _options);
}