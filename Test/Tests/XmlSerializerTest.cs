using Core;
using Engine.Serialization;

namespace Test.Tests;

public sealed class XmlSerializerTest : SerializerTestBase
{
    public override string Name => nameof(XmlSerializerTest);
    protected override string Extension => ".xml";
    protected override IStorageSerializer Serializer { get; } = new XmlSerializerStrategy();
}