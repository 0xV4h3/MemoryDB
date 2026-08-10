using System.Collections;
using Core;

namespace Engine.Serialization.Binary;

public sealed class BinarySerializerStrategy : IStorageSerializer
{
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
        return new object();
    }
}