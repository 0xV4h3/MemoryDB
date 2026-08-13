using System.Collections;
using Core;
using Engine.Serialization.Binary.Cache;
using Engine.Serialization.Binary.Utils;
using Engine.Serialization.Binary.Compression;
using Engine.Serialization.Binary.Metadata;

namespace Engine.Serialization.Binary;

public sealed class BinarySerializerStrategy(
    ICompressionStrategy? compression = null, 
    IIntegrityChecksum? checksum = null) : IStorageSerializer
{
    public const int CurrentFormatVersion = 1;

    private readonly ICompressionStrategy _compression = compression ?? new NoCompression();
    private readonly IIntegrityChecksum _checksum = checksum ?? new Crc32Checksum();
    
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

    private void WriteValue(BinaryWriter writer, object? value, Type declaredType)
    {
        bool canBeNull = !declaredType.IsValueType || Nullable.GetUnderlyingType(declaredType) != null;

        if (canBeNull) 
        {
            writer.Write(value != null);
        }
        
        if (value is not { } nonNullValue) 
            return; 

        var (kind, elementType, underlyingType) = FieldKindClassifier.Classify(declaredType);

        switch (kind)
        {
            case FieldKind.String:
                writer.Write((string)nonNullValue);
                break;
            case FieldKind.Guid:
                writer.Write(((Guid)nonNullValue).ToByteArray());
                break;
            case FieldKind.DateTime:
                writer.Write(((DateTime)nonNullValue).ToBinary());
                break;
            case FieldKind.TimeSpan:
                writer.Write(((TimeSpan)nonNullValue).Ticks);
                break;
            case FieldKind.Enum:
                WritePrimitive(writer, Convert.ChangeType(value, underlyingType!));
                break;
            case FieldKind.Nullable:
                WriteValue(writer, value, underlyingType!);
                break;
            case FieldKind.Primitive:
                WritePrimitive(writer, value);
                break;
            case FieldKind.Array:
            case FieldKind.List:
                WriteCollection(writer, (IEnumerable)value, elementType!);
                break;
            case FieldKind.Nested:
                WriteNested(writer, value);
                break;
            default:
                throw new NotSupportedException($"Type '{declaredType}' is not supported by BinarySerializerStrategy.");
        }
    }
    
    private static void WritePrimitive(BinaryWriter writer, object value)
    {
        switch (value)
        {
            case bool v: writer.Write(v); break;
            case byte v: writer.Write(v); break;
            case sbyte v: writer.Write(v); break;
            case short v: writer.Write(v); break;
            case ushort v: writer.Write(v); break;
            case int v: writer.Write(v); break;
            case uint v: writer.Write(v); break;
            case long v: writer.Write(v); break;
            case ulong v: writer.Write(v); break;
            case float v: writer.Write(v); break;
            case double v: writer.Write(v); break;
            case decimal v: writer.Write(v); break;
            case char v: writer.Write(v); break;
            default:
                throw new NotSupportedException($"Unsupported primitive type: {value.GetType()}");
        }
    }

    private void WriteCollection(BinaryWriter writer, IEnumerable value, Type elementType)
    {
        var items = value.Cast<object>().ToList();
        writer.Write(items.Count);
        foreach (var item in items)
            WriteValue(writer, item, elementType);
    }

    private void WriteNested(BinaryWriter writer, object value)
    {
        var plan = TypeAccessorCache.GetOrBuild(value.GetType());
        foreach (var accessor in plan.Properties)
            WriteValue(writer, accessor.Getter(value), accessor.PropertyType);
    }
    
    private object? ReadValue(BinaryReader reader, Type declaredType)
    {
        bool canBeNull = !declaredType.IsValueType || Nullable.GetUnderlyingType(declaredType) != null;

        if (canBeNull) 
        {
            bool hasValue = reader.ReadBoolean();
            if (!hasValue) return null;
        }
        
        var (kind, elementType, underlyingType) = FieldKindClassifier.Classify(declaredType);
        
        return kind switch
        {
            FieldKind.String => reader.ReadString(),
            FieldKind.Guid => new Guid(reader.ReadBytes(16)),
            FieldKind.DateTime => DateTime.FromBinary(reader.ReadInt64()),
            FieldKind.TimeSpan => TimeSpan.FromTicks(reader.ReadInt64()),
            FieldKind.Enum => Enum.ToObject(declaredType, ReadPrimitive(reader, underlyingType!)),
            FieldKind.Nullable => ReadValue(reader, underlyingType!),
            FieldKind.Primitive => ReadPrimitive(reader, declaredType),
            FieldKind.Array => ReadCollection(reader, isArray: true, elementType!),
            FieldKind.List => ReadCollection(reader, isArray: false, elementType!),
            FieldKind.Nested => ReadNested(reader, declaredType),
            _ => throw new NotSupportedException($"Type '{declaredType}' is not supported by BinarySerializerStrategy.")
        };
    }
    
    private static object ReadPrimitive(BinaryReader reader, Type type)
    {
        if (type == typeof(bool)) return reader.ReadBoolean();
        if (type == typeof(byte)) return reader.ReadByte();
        if (type == typeof(sbyte)) return reader.ReadSByte();
        if (type == typeof(short)) return reader.ReadInt16();
        if (type == typeof(ushort)) return reader.ReadUInt16();
        if (type == typeof(int)) return reader.ReadInt32();
        if (type == typeof(uint)) return reader.ReadUInt32();
        if (type == typeof(long)) return reader.ReadInt64();
        if (type == typeof(ulong)) return reader.ReadUInt64();
        if (type == typeof(float)) return reader.ReadSingle();
        if (type == typeof(double)) return reader.ReadDouble();
        if (type == typeof(decimal)) return reader.ReadDecimal();
        if (type == typeof(char)) return reader.ReadChar();
        
        throw new NotSupportedException($"Unsupported primitive type: {type}");
    }
    private object ReadCollection(BinaryReader reader, bool isArray, Type elementType)
    {
        int count = reader.ReadInt32();
        
        if (isArray)
            return ReadArray(reader, elementType, count);

        return ReadList(reader, elementType, count);
    }

    private Array ReadArray(BinaryReader reader, Type elementType, int count)
    {
        var array = Array.CreateInstance(elementType, count);
        for (int i = 0; i < count; i++)
            array.SetValue(ReadValue(reader, elementType), i);
        return array;
    }
    
    private object ReadList(BinaryReader reader, Type elementType, int count)
    {
        var listType = typeof(List<>).MakeGenericType(elementType);
        var list = (IList)Activator.CreateInstance(listType)!;
        for (int i = 0; i < count; i++)
            list.Add(ReadValue(reader, elementType));
        return list;
    }

    private object ReadNested(BinaryReader reader, Type type)
    {
        var instance = Activator.CreateInstance(type)!;
        var plan = TypeAccessorCache.GetOrBuild(type);

        foreach (var accessor in plan.Properties)
            accessor.Setter(instance, ReadValue(reader, accessor.PropertyType));

        return instance;
    }
}