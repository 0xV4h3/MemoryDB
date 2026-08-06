using System.Xml.Serialization;
using Core;

namespace Engine.Serialization;

public class XmlSerializerStrategy : IStorageSerializer
{
    private static readonly Dictionary<Type, XmlSerializer> _serializerCache = [];

    public byte[] Serialize<T>(T data) where T : class
    {
        using var ms = new MemoryStream();
        GetSerializer(typeof(T)).Serialize(ms, data);
        return ms.ToArray();
    }

    public T? Deserialize<T>(byte[] bytes) where T : class
    {
        using var ms = new MemoryStream(bytes);
        return (T?)GetSerializer(typeof(T)).Deserialize(ms);
    }

    private static XmlSerializer GetSerializer(Type type)
    {
        if (_serializerCache.TryGetValue(type, out var cached)) return cached;

        var created = new XmlSerializer(type);
        _serializerCache[type] = created;
        return created;
    }
}