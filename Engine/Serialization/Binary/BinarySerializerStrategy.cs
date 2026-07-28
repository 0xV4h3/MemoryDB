using Core;

namespace Engine.Serialization.Binary;

public sealed class BinarySerializerStrategy : IStorageSerializer
{
    public string SupportedExtension => ".bin";
    
    public byte[] Serialize<T>(T data) where T : class
    {
        using var ms = new MemoryStream();
        using var writer = new BinaryWriter(ms);

        WriteValue(writer, data, typeof(T));
        writer.Flush();
        return ms.ToArray();
    }

    public T? Deserialize<T>(byte[] bytes) where T : class
    {
        ArgumentNullException.ThrowIfNull(bytes);
        if (bytes.Length == 0) return null;

        using var ms = new MemoryStream(bytes);
        using var reader = new BinaryReader(ms);

        return (T?)ReadValue(reader, typeof(T));
    }
    
    private void WriteValue(BinaryWriter writer, object? value, Type declaredType) { }

    private object? ReadValue(BinaryReader reader, Type declaredType)
    {
        return new object();
    }
}