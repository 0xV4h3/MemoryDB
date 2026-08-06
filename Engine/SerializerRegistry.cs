using Core;

namespace Engine;

public sealed class SerializerRegistry
{
    private readonly Dictionary<string, IStorageSerializer> _byExtension = [];

    public SerializerRegistry Register(string extension, IStorageSerializer serializer)
    {
        _byExtension[extension] = serializer;
        return this;
    }

    public IEnumerable<(string Extension, IStorageSerializer Serializer)> All =>
        _byExtension.Select(kvp => (kvp.Key, kvp.Value));

    public IStorageSerializer? GetByExtension(string extension) =>
        _byExtension.GetValueOrDefault(extension);
}