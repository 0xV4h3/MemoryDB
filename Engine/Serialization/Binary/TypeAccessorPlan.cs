namespace Engine.Serialization.Binary;

internal sealed class TypeAccessorPlan
{
    public required Type Type { get; init; }
    public required PropertyAccessor[] Properties { get; init; }
}