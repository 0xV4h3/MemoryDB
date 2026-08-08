using Core;
using Engine.Serialization;

namespace Test.Tests;

public sealed class JsonSerializerTest : SerializerTestBase
{
    public override string Name => nameof(JsonSerializerTest);
    protected override string Extension => ".json";
    protected override IStorageSerializer Serializer { get; } = new JsonSerializerStrategy();
}