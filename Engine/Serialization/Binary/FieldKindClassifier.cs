using System.Collections;

namespace Engine.Serialization.Binary;

internal class FieldKindClassifier
{
    public static (FieldKind Kind, Type? ElementType, Type? UnderlyingType) Classify(Type type)
    {
        if (type == typeof(string)) return (FieldKind.String, null, null);
        if (type == typeof(Guid)) return (FieldKind.Guid, null, null);
        if (type == typeof(DateTime)) return (FieldKind.DateTime, null, null);
        if (type == typeof(TimeSpan)) return (FieldKind.TimeSpan, null, null);

        var nullableUnderlying = Nullable.GetUnderlyingType(type);
        if (nullableUnderlying != null) return (FieldKind.Nullable, null, nullableUnderlying);

        if (type.IsEnum) return (FieldKind.Enum, null, Enum.GetUnderlyingType(type));
        if (type.IsPrimitive || type == typeof(decimal)) return (FieldKind.Primitive, null, null);

        if (type.IsArray) return (FieldKind.Array, type.GetElementType(), null);

        if (type.IsGenericType && typeof(IEnumerable).IsAssignableFrom(type))
        {
            var elementType = type.GetGenericArguments().FirstOrDefault() ?? typeof(object);
            return (FieldKind.List, elementType, null);
        }

        return (FieldKind.Nested, null, null);
    }
}