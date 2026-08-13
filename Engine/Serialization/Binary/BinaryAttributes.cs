namespace Engine.Serialization.Binary;

[AttributeUsage(AttributeTargets.Property)]
public sealed class BinaryIgnoreAttribute : Attribute;

[AttributeUsage(AttributeTargets.Property)]
public sealed class BinaryOrderAttribute(int order) : Attribute
{
    public int Order { get; } = order;
}