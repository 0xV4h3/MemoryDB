using Core;
using Engine.Serialization.Binary;

namespace Test.Tests;

public sealed class BinarySerializerTest : SerializerTestBase
{
    public override string Name => nameof(BinarySerializerTest);
    protected override string Extension => ".bin";
    protected override IStorageSerializer Serializer { get; } = new BinarySerializerStrategy();
}