namespace Engine.Serialization.Binary;

internal enum FieldKind
{
    Primitive,
    String,
    Guid,
    DateTime,
    TimeSpan,
    Enum,
    Nullable,
    Array,
    List,
    Nested
}